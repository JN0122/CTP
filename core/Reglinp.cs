using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearRegression;

namespace CTP.core
{
    internal class Reglinp
    {
        public static double FitLine(double[] x, double[] y)
        {
            (double slope, double intercept) = SimpleRegression.Fit(x, y);

            Tuple<double, double> regression = Tuple.Create(slope, intercept);

            double slopeValue = regression.Item1;
            double interceptValue = regression.Item2;

            return interceptValue; // interceptValue lub slopeValue idk xd
        }

    }
}
