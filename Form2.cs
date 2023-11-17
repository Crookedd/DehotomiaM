using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Diagnostics;

namespace DehotomiaM
{
    public partial class Form2 : Form
    {
        string[,] list = new string[50, 50];
        public Form2()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView()
        {
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Name = "Число";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = ExportExcel();
            dataGridView1.Rows.Clear();
            string s;
            for (int i = 0; i < n; i++) // по всем строкам
            {
                s = "";
                for (int j = 0; j < 50; j++) //по всем колонкам
                    s += list[i, j];
                dataGridView1.Rows.Add(s);
            }

        }
        private int ExportExcel()
        {
            // Выбрать путь и имя файла в диалоговом окне
            OpenFileDialog ofd = new OpenFileDialog();
            // Задаем расширение имени файла по умолчанию (открывается папка с программой)
            ofd.DefaultExt = "*.xls;*.xlsx";
            // Задаем строку фильтра имен файлов, которая определяет варианты
            ofd.Filter = "файл Excel (Spisok.xlsx)|*.xlsx";
            // Задаем заголовок диалогового окна
            ofd.Title = "Выберите файл базы данных";
            if (!(ofd.ShowDialog() == DialogResult.OK)) // если файл БД не выбран -> Выход
                return 0;
            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(ofd.FileName);
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];//получить 1-й лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//последнюю ячейку
                                                                                                // размеры базы
            int lastColumn = (int)lastCell.Column;
            int lastRow = (int)lastCell.Row;
            // Перенос в промежуточный массив класса Form1: string[,] list = new string[50, 5]; 
            for (int j = 0; j < 5; j++) //по всем колонкам
                for (int i = 0; i < lastRow; i++) // по всем строкам
                    list[i, j] = ObjWorkSheet.Cells[i + 1, j + 1].Text.ToString(); //считываем данные
            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из Excel
            GC.Collect(); // убрать за собой
            return lastRow;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateRandomData();
        }
        private void GenerateRandomData()
        {
            dataGridView1.Rows.Clear();
            var RandomNumber = new Random((int)Stopwatch.GetTimestamp());
            for (int i = 0; i < 10; i++)
            {
                double number;
                number = RandomNumber.Next(-10, 10) * 3 - 10 + 15;
                dataGridView1.Rows.Add(number);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SortNumbers(SortOrder.Ascending);
        }
        private void QuickSortWrapper(List<double> list, SortOrder sortOrder)
        {
            List<double> copy = new List<double>(list);
            QuickSort(copy, 0, copy.Count - 1, sortOrder);
        }

        public struct SortStats
        {
            public double Time { get; set; }
            public int Iterations { get; set; }
        }
        private void SortNumbers(SortOrder sortOrder)
        {  // выбрана ли хотя бы однасортировка?
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked &&
                !checkBox4.Checked && !checkBox5.Checked)
            {
                MessageBox.Show("Отсутствуют данные для сортировки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<double> dataGridViewNumbers = new List<double>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && double.TryParse(row.Cells[0].Value.ToString(), out double number))
                {
                    dataGridViewNumbers.Add(number);
                }
            }
            Dictionary<string, SortStats> sortStats = new Dictionary<string, SortStats>();

            if (checkBox1.Checked)
            {
                sortStats["Пузырьковая"] = MeasureSortingStats(() => BubbleSort(dataGridViewNumbers, sortOrder));
            }
            if (checkBox5.Checked)
            {
                sortStats["Вставками"] = MeasureSortingStats(() => InsertionSort(dataGridViewNumbers, sortOrder));
            }
            if (checkBox3.Checked)
            {
                sortStats["Шейкерная"] = MeasureSortingStats(() => ShakerSort(dataGridViewNumbers, sortOrder));
            }
            if (checkBox2.Checked)
            {
                sortStats["Быстрая"] = MeasureSortingStats(() => QuickSortWrapper(dataGridViewNumbers, sortOrder));
            }
            if (checkBox4.Checked)
            {
                sortStats["BOGO"] = MeasureSortingStats(() => BogoSort(dataGridViewNumbers, sortOrder));
            }
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var kvp in sortStats)
            {
                resultBuilder.AppendLine($"{kvp.Key}: \r\nВремя выполнения - {kvp.Value.Time} нс, Количество итераций - {kvp.Value.Iterations}");
            }
            textBox1.Clear();
            string text = "";
            for (int i = 0; i < dataGridViewNumbers.Count; i++)
            {
                text += dataGridViewNumbers[i].ToString() + " ";
            }
            textBox1.Text = "Отсортированный массив :" + "\r\n\r\n" + text + "\r\n\r\n" + "Результаты сортировок: " + "\r\n\r\n" + resultBuilder;
        }
        private SortStats MeasureSortingStats(Action sortingAction)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            sortingAction();
            stopwatch.Stop();
            double time = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000000000;
            return new SortStats { Time = time, Iterations = count };
        }

        private void UpdateChart(List<double> list)
        {
            chart1.Series.Clear();
            chart1.Series.Add("Numbers");

            foreach (var number in list)
            {
                chart1.Series["Numbers"].Points.AddY(number);
            }

            chart1.Invalidate();
        }

        int count;
        private void BubbleSort(List<double> list, SortOrder sortOrder)
        {

            int n = list.Count;
            double temp;
            for (int i = 0; i < n - 1; i++)
            {
                //count++;
                for (int j = 0; j < n - i - 1; j++)
                {
                    ++count;
                    if ((sortOrder == SortOrder.Ascending && list[j] > list[j + 1]) ||
                        (sortOrder == SortOrder.Descending && list[j] < list[j + 1]))
                    {
                        temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;

                        UpdateChart(list);

                    }
                }
                count++;
            }
            //MessageBox.Show($"Iterations: {count}");
        }


        private void InsertionSort(List<double> list, SortOrder sortOrder)
        {
            // начинаем со второго элемента (элемент с индексом 0
            // уже отсортировано)
            double n = list.Count;
            for (int i = 1; i < n; i++)
            {
                double k = list[i];
                int j = i - 1;

                while ((j >= 0 && sortOrder == SortOrder.Ascending && list[j] > k) ||
                       (j >= 0 && sortOrder == SortOrder.Descending && list[j] > k))
                {
                    list[j + 1] = list[j];
                    list[j] = k;
                    j--;
                    UpdateChart(list);
                    count++;
                }
            }
        }

        private void QuickSort(List<double> list, int left, int right, SortOrder sortOrder)
        {
            if (left < right)
            {
                int pivot = Partition(list, left, right, sortOrder);

                QuickSort(list, left, pivot - 1, sortOrder);
                QuickSort(list, pivot + 1, right, sortOrder);
            }
            UpdateChart(list);
            count++;
        }

        //Функция для нахождения основного элемена
        static int Partition(List<double> list, int left, int right, SortOrder sortOrder)
        {
            double pivot = list[right];
            int i = (left - 1);

            for (int j = left; j < right; j++)
            {
                if ((sortOrder == SortOrder.Ascending && list[j] <= pivot) ||
                    (sortOrder == SortOrder.Descending && list[j] <= pivot))
                {
                    i++;
                    double temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
            double temp1 = list[i + 1];
            list[i + 1] = list[right];
            list[right] = temp1;

            return i + 1;
        }

        private void ShakerSort(List<double> list, SortOrder sortOrder)
        {
            int left = 0;
            int right = list.Count - 1;
            bool swapped = true;
            while (left < right && swapped)
            {
                swapped = false;
                for (int i = left; i < right; ++i)
                {
                    if ((sortOrder == SortOrder.Ascending && list[i] > list[i + 1]) ||
                        (sortOrder == SortOrder.Descending && list[i] > list[i + 1]))
                    {
                        Swap(list, i, i + 1);
                        swapped = true;
                    }
                }
                --right;
                for (int i = right; i > left; --i)
                {
                    if ((sortOrder == SortOrder.Ascending && list[i] < list[i - 1]) ||
                        (sortOrder == SortOrder.Ascending && list[i] < list[i - 1]))
                    {
                        Swap(list, i, i - 1);
                        swapped = true;
                    }
                }
                ++left;
                UpdateChart(list);
                count++;
            }
        }
        static void Swap(List<double> list, int i, int j)
        {
            double temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        private void BogoSort(List<double> list, SortOrder sortOrder)
        {
            Random random = new Random();

            //Проверка упорядоченности массива

            while (!IsSorted(list, sortOrder))
            {
                Shuffle(list, random);
            }
            UpdateChart(list);
            count++;
        }
        //Встряхиваем рандомно все значения, в надежде,что все значения встанут правильно. 
        static void Shuffle(List<double> list, Random random)//, Random random)
        {
            int n = list.Count;
            //Random rand = new Random();
            while (n > 1)
            {
                --n;
                int randomIndex = random.Next(n + 1);
                double temp = list[randomIndex];
                list[randomIndex] = list[n];
                list[n] = temp;
            }
        }

        //Проверяем отсортирован ли массив? 
        static bool IsSorted(List<double> list, SortOrder sortOrder)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if ((sortOrder == SortOrder.Ascending && list[i - 1] > list[i]) ||
                    (sortOrder == SortOrder.Descending && list[i - 1] < list[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
