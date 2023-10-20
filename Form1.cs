using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DehotomiaM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double F(double x)
        {
            double f;
            f = (27 - 18 * x + 2 * Math.Pow(x, 2)) * Math.Exp(-(x / 3));

            return f;
        }
        double Proisvodnaya(double x)
        {
            double y = -1 * (2 * x * x * Math.Exp(-x / 3) / 3) + 10 * x * Math.Exp(-x / 3) - 27 * Math.Exp(-x / 3);
            return y;
        }

       /* double RootX(Func<double, double> f, double a, double b, double epsilon)
        {
            double Root;
            if (F(a) * F(b) <= 0)
            {
                MessageBox.Show("Условие сходимости выполнено");
                Root = (a + b) / 2;

                while (Math.Abs(b - a) > Math.Pow(10, -epsilon))
                {
                    double y1 = F(a), y2 = F(b), y3 = F(Root);
                    if (y1 * y3 < 0)
                    {
                        b = Root;
                    }
                    else if (y2 * y3 < 0)
                    {
                        a = Root;
                    }
                    else
                    {
                        break;
                    }
                    Root = (a + b) / 2;
                }
                if ((27 - 18 * Root + 2 * Math.Pow(Root, 2)) * Math.Exp(-Root / 3) < 0 + Root && (27 - 18 * Root + 2 * Math.Pow(Root, 2)) * Math.Exp(-Root / 3) > 0 - Root)
                {
                    return Math.Round(Root); ;
                }
            }
            else
            {
                throw new ArgumentException("Условие сходимости не выполнено");
            }

            return 0;
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double a, b, Xi;
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                if (a >= b)
                {
                    throw new ArgumentException("Некорректные границы интервала");
                }
                this.chart1.Series[0].Points.Clear();
                double x = a;
                double y;
                while (x <= b)
                {
                    y = F(x);
                    this.chart1.Series[0].Points.AddXY(x, y);
                    x += 0.1;
                }
                double Root;
                if (F(a) * F(b) <= 0)
                {
                    MessageBox.Show("Условие сходимости выполнено");
                    Root = (a + b) / 2;

                    while (Math.Abs(b - a) > Math.Pow(10, -Xi))
                    {
                        double y1 = F(a), y2 = F(b), y3 = F(Root);
                        if (y1 * y3 < 0)
                        {
                            b = Root;
                        }
                        else if (y2 * y3 < 0)
                        {
                            a = Root;
                        }
                        else
                        {
                            break;
                        }
                        Root = (a + b) / 2;
                    }
                    if ((27 - 18 * Root + 2 * Math.Pow(Root, 2)) * Math.Exp(-Root  / 3) < 0 + Root && (27 - 18 * Root + 2 * Math.Pow(Root, 2)) * Math.Exp(-Root / 3) > 0 - Root)
                    {
                        MessageBox.Show("Корень равен " + Root + ".");
                    }
                }
                else
                {
                    throw new ArgumentException("Условие сходимости не выполнено");
                }
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                double max, min;
                double delta = Xi / 10;
                while (b - a >= Xi)
                {
                    double middle = (a + b) / 2;
                    double lambda = middle - delta, mu = middle + delta;
                    if (F(lambda) < F(mu))
                        b = mu;
                    else
                        a = lambda;
                }
                min = (a + b) / 2;


                // Точка максимума
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                while (b - a >= Xi)
                {
                    double middle = (a + b) / 2;
                    double lambda = middle - delta, mu = middle + delta;
                    if (F(lambda) > F(mu))
                        b = mu;
                    else
                        a = lambda;
                }
                max = (a + b) / 2;

                MessageBox.Show($"Локальный минимум {min}; максимум {max};");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        double GoldenSectionSearchMin(Func<double, double> f, double a, double b, double epsilon)
        {
            double phi = (1 + Math.Sqrt(5)) / 2; // Золотое сечение

            double x1 = b - (b - a) / phi;
            double x2 = a + (b - a) / phi;
            double f1 = f(x1);
            double f2 = f(x2);

            while (Math.Abs(b - a) > epsilon)
            {
                if (f1 < f2)
                {
                    b = x2;
                    x2 = x1;
                    f2 = f1;
                    x1 = b - (b - a) / phi;
                    f1 = f(x1);
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    f1 = f2;
                    x2 = a + (b - a) / phi;
                    f2 = f(x2);
                }
            }

            return (a + b) / 2;
        }
        double GoldenSectionSearchMax(Func<double, double> f, double a, double b, double epsilon)
        {
            double phi = (1 + Math.Sqrt(5)) / 2; // Золотое сечение

            double x1 = b - (b - a) / phi;
            double x2 = a + (b - a) / phi;
            double f1 = f(x1);
            double f2 = f(x2);

            while (Math.Abs(b - a) > epsilon)
            {
                if (f1 > f2)
                {
                    b = x2;
                    x2 = x1;
                    f2 = f1;
                    x1 = b - (b - a) / phi;
                    f1 = f(x1);
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    f1 = f2;
                    x2 = a + (b - a) / phi;
                    f2 = f(x2);
                }
            }

            return (a + b) / 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double a, b, Xi;
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }
                if (a >= b)
                {
                    throw new ArgumentException("Некорректные границы интервала");
                }
                this.chart1.Series[0].Points.Clear();
                double x = a;
                double y;
                while (x <= b)
                {
                    y = F(x);
                    this.chart1.Series[0].Points.AddXY(x, y);
                    x += 0.1;
                }
                double minimum = GoldenSectionSearchMin(F, a, b, Xi);
                double max = GoldenSectionSearchMax(F, a, b, Xi);

                MessageBox.Show($"Локальный минимум: x = {minimum},Локальный максимум: x = {max} f(x) = {F(minimum)}");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }

        }
    }
}
