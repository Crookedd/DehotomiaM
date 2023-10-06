using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            double a = Convert.ToDouble(textBox1.Text);
            double b = Convert.ToDouble(textBox2.Text);
            double Xi = Convert.ToDouble(textBox3.Text);
            double max;
            if (F(a) > F(b))
            {
                max = F(a);
            }
            else
            {
                max = F(b);
            }
            double answer = 1.0;
            while (CheckPrecision(Proisvodnaya((a + b) / 2), Xi))
            {
                answer = F((double)((a + b) / 2));
                if (Proisvodnaya((double)((a + b) / 2)) * Proisvodnaya(a) > 0)
                {
                    a = (double)((a + b) / 2);
                }
                else
                {
                    b = (double)((a + b) / 2);
                }
            }
            MessageBox.Show($"минимум {answer}; максимум {max}");
            this.chart1.Series[0].Points.Clear();
            double x = 0;
            double y;
            while (x <= 8)
            {
                y = F(x);
                this.chart1.Series[0].Points.AddXY(x, y);
                x += 0.1;
            }
        }
        public static bool CheckPrecision(double yStr, double precision)
        {
            bool check = false;
            string myY = Convert.ToString(yStr);
            string myPres = Convert.ToString(precision);
            for (int pos = 2; pos < myPres.Length - 1; ++pos)
            {
                if (myY[pos] != '0')
                {
                    check = true; break;
                }
            }
            return check;
        }
    }
}
