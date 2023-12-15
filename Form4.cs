using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace DehotomiaM
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        string[,] list = new string[50, 50];

        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            dataGridViewA.Rows.Clear();
            dataGridViewA.Columns.Clear();
          //  dataGridViewB.Rows.Clear();
            if (!int.TryParse(textBox1.Text, out n))
            {
                throw new ArgumentException("Некорректные значения входных данных");
            }
            dataGridViewA.ColumnCount = n;
            dataGridViewA.RowCount = n;
           // dataGridViewB.RowCount = n;
            var RandomNumber = new Random((int)Stopwatch.GetTimestamp());
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double number;
                    number = RandomNumber.Next(-50, 50) - 10 + 15;
                    dataGridViewA.Rows[i].Cells[j].Value = i + number * j + number;
                }
                dataGridViewA.Rows[i].Cells[n].Value = list[i, n];
                dataGridViewA.Columns[n].HeaderCell.Value = "B";
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
            int n = ExportExcel();
            dataGridViewA.Rows.Clear();
            dataGridViewA.ColumnCount = n + 1;
            dataGridViewA.RowCount = n + 1;
            for (int i = 0; i < n; i++) // по всем строкам
            {
                for (int j = 0; j < n; j++)
                {
                    dataGridViewA.Rows[i].Cells[j].Value = list[i, j];
                }//по всем колонкам
                dataGridViewA.Columns[i].HeaderCell.Value = $"X{i + 1}";
                dataGridViewA.Rows[i].Cells[n].Value = list[i, n];
                dataGridViewA.Columns[n].HeaderCell.Value = "B";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int n = ExportExcel();
            dataGridViewB.Rows.Clear();
            dataGridViewB.RowCount = n; 
            for (int i = 0; i < n; i++) // по всем строкам
            {
                dataGridViewB.Rows[i].Cells[0].Value = list[i, 0];
                dataGridViewB.Columns[0].HeaderCell.Value = "B";
            }
        }
        public void myCauss(ref double[,] a, ref double[] b, ref double[] x, int n)
        {
            for ( int k = 0; k< n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = k + 1; j < n; j++)
                    {
                        a[i, j] = a[i, j] - a[k, j] * (a[i, k] / a[k, k]);

                    }
                    b[i] = b[i] - b[k] * a[i, k] / a[k, k];
                }
            }
            double sum = 0;
            for(int k = n -1; k >= 0;k--)
            {
                sum = 0;
                for(int j = k + 1; j < n; j++)
                {
                    sum = sum + a[k, j] * x[j];
                }
                x[k] = (b[k] - sum) / a[k, k];
            }
            for( int i = 0; i < n; i++)
            {
                textBox2.Text = $"x{i + 1} = {x[i].ToString("F2")}";
            }
        }
        public void SOLVE(double[,] pA, double[] pb)
        {   
            //  double[] answer = new double[] { };

            //СЛАУ выглядит вот так: A*x = b. A - матрица коэф., b - вектор решений
            double d = determinant(pA);
            List<double[,]> proc = new List<double[,]> { };

            List<double> answer = new List<double> { };

            for (int f = 0; f < pb.Length; f++)
            {
                answer.Add(determinant(proc[f]) / d);
            }
            if (d == 0.0)
                answer.Add(1.0);
            else
                answer.Add(0.0);

            for (int i = 0; i < answer.Count; i++)
            {
                textBox2.Text = $"x{i + 1} = {answer[i].ToString("F2")}";
            }
        }
        static double determinant(double[,] m)
        {
            int numRows = m.GetLength(0);
            int numCols = m.GetLength(1);
            int n = numCols;
            double[,] matrix = new double[n, n];
            matrix = m;

            for (int k = 1; k < n; k++)
            {
                for (int i = k; i < n; i++)
                {
                    double C = matrix[i, k - 1] / matrix[k - 1, k - 1];
                    for (int j = 0; j < numCols; j++)
                    {
                        matrix[i, j] -= C * matrix[k - 1, j];
                    }
                }
            };
            double result = 1;
            for (int i = 0; i < n; i++)
            {
                result *= matrix[i, i];
            };
            return result;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox1.Text, out n))
            {
                throw new ArgumentException("Некорректные значения входных данных");
            }

            if (checkBox2.Checked)
            {
                double[,] A = new double[n, n];
                double[] B = new double[n];
                double[] X = new double[n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        A[i, j] = Convert.ToInt32(dataGridViewA[j, i].Value);
                    }
                    B[i] = Convert.ToInt32(dataGridViewA[n, i].Value);
                }
                myCauss(ref A, ref B, ref X, n);
            }
            if (checkBox1.Checked)
            {
                double[,] A = new double[n, n];
                double[] B = new double[n];
                double[] X = new double[n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        A[i, j] = Convert.ToInt32(dataGridViewA[j, i].Value);
                    }
                    B[i] = Convert.ToInt32(dataGridViewA[n, i].Value);
                }
                SOLVE(A,B);

            }

        }

        private void Form4_Load(object sender, EventArgs e)
        {
        }
    }
}
