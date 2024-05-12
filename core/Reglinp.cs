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
        public static float FitLine(double[] x, double[] y)
        {
            (double slope, double intercept) = SimpleRegression.Fit(x, y);

            Tuple<double, double> regression = Tuple.Create(slope, intercept);

            float slopeValue = (float)regression.Item1;
            float interceptValue = (float)regression.Item2;

            return interceptValue; // interceptValue lub slopeValue - nie jestem pewien xd
        }

    }
}

