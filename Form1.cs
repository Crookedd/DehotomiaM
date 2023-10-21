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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            double y = (4*x-18)*Math.Exp(-(x/3)) - ((2 * Math.Pow(x, 2) - 18 * x + 27) * Math.Exp(-(x / 3)))/3;
            return y;
        }

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
                Xi = (int)-Math.Log10(Xi);
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
                        textBox4.Text = Root.ToString(); ;
                    }
                }
                else
                {
                    throw new ArgumentException("Нет корней на этом интервале или их больше одного");
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

                textBox5.Text = min.ToString();
                textBox6.Text = max.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        double MetodNewton(double a, double b, double epsilon)
        {
            double x0 = (a + b) / 2; // Начальное приближение

            while (Math.Abs(Proisvodnaya(x0)) > epsilon)
            {
                x0 = x0 - Proisvodnaya(x0) / F(x0);
            }

            return x0;
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
                Xi = (int)-Math.Log10(Xi);
                this.chart1.Series[0].Points.Clear();
                double x = a;
                double y;
                while (x <= b)
                {
                    y = F(x);
                    this.chart1.Series[0].Points.AddXY(x, y);
                    x += 0.1;
                }

                double minimum = MetodNewton(a, b, Xi);
                double max = MetodNewton(a, b, -Xi);

                MessageBox.Show($"Локальный минимум: x = {minimum},Локальный максимум: x = {max};");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }

        }
    }
}
