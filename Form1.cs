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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double a, b, Xi;
                if (!double.TryParse(textBox1.Text, out a) || !double.TryParse(textBox2.Text, out b) || !double.TryParse(textBox3.Text, out Xi))
                {
                    throw new ArgumentException("Некорректные значения входных данных");
                }

                double max, min;
                double delta = Xi / 10;

                if (a >= b)
                {
                    throw new ArgumentException("Некорректные границы интервала");
                }

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


                while (b - a >= Xi)
                {
                    double middle = (a + b) / 2;
                    double lambda = middle - delta, mu = middle + delta;
                    if (-F(lambda) < -F(mu))
                        b = mu;
                    else
                        a = lambda;
                }
                max = (a + b) / 2;


                MessageBox.Show($"минимум {min}; максимум {max}");
                this.chart1.Series[0].Points.Clear();
                double x = a;
                double y;
                while (x <= b)
                {
                    y = F(x);
                    this.chart1.Series[0].Points.AddXY(x, y);
                    x += 0.1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
