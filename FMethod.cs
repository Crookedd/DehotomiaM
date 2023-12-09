using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehotomiaM
{
    internal class FMethod
    {
        public double LeftRectangle(double A, double B, double N, Func<double, double> F)
        {
            double h = (B - A) / N;
            double sum = 0d;
            double x = 0d;
            double y = 0d;

            for (int i = 0; i <= N - 1; i++)
            {
                x = A + i * h;
                y = F(x);
                sum += y;
            }
            double result = h * sum;
            return result;
        }
        private double RightRectangle(double A, double B, double E, double N, Func<double, double> F)
        {
            double h = (B - A) / N;
            double sum = 0d;
            double x = 0d;
            double y = 0d;

            for (var i = 1; i <= N; i++)
            {
                x = A + i * h;
                y = F(x);
                sum += y;
            }
            double result = h * sum;
            return result;
        }
        private double CentralRectangle(double A, double B, double E, double N, Func<double, double> F)
        {
            double h = (B - A) / N;
            double sum = (F(A) + F(B)) / 2;
            double x = 0d;
            double y = 0d;

            for (var i = 1; i < N; i++)
            {
                x = A + h * i;
                y = F(x);
                sum += y;
            }
            double result = h * sum;
            return result;
        }
        public double Simpson(double A, double B, double N, Func<double, double> F)
        {

            double h = (B - A) / N;
            double sum1 = 0d;
            double sum2 = 0d;
            double xk = 0d, yk = 0d, xk_1 = 0d;
            for (double k = 1; k <= N; k++)
            {
                xk = A + (k * h);
                if (k <= N - 1)
                {
                    yk = F(xk);
                    sum1 += yk;
                }
                xk_1 = A + ((k - 1) * h);
                sum2 += F((xk + xk_1) / 2);
            }
            double result = h / 3d * ((1d / 2d * F(A)) + sum1 + (2 * sum2) + (1d / 2d * F(B)));
            return result;
        }
        public double Trapezoidal(double A, double B, double N, Func<double, double> F)
        {
            double h = (B - A) / N;
            double sum = 0d;
            double xk = 0d, yk = 0d;
            for (double k = 1; k <= N - 2; k++)
            {
                xk = A + (k * h);
                yk = F(xk);
                sum += yk;
            }
            xk = A + ((N - 1) * h);

            return h / 2d * (F(A) + F(xk)) + (h * sum);
        }
    }
}
