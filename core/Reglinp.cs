using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.core
{
    internal class Reglinp
    {
        public static double FitLine(double[] x, double[] y)
        {
            if (x.Length != y.Length)
            {
                throw new ArgumentException("x and y arrays must have the same length");
            }

            int n = x.Length;
            double sumX = 0;
            double sumY = 0;
            double sumXY = 0;
            double sumX2 = 0;

            for (int i = 0; i < n; i++)
            {
                sumX += x[i];
                sumY += y[i];
                sumXY += x[i] * y[i];
                sumX2 += x[i] * x[i];
            }

            // Calculate slope (m) and intercept (b)
            double slope = (sumXY - sumX * sumY / n) / (sumX2 - sumX * sumX / n);
            double intercept = (sumY - slope * sumX) / n;

            System.Diagnostics.Trace.WriteLine($"Slope (m): {slope}");
            System.Diagnostics.Trace.WriteLine($"Intercept (b): {intercept}");

            return intercept;
        }
    }
}
