using System;

namespace MathEvaluator.Core
{
    public static class MathEx
    {
        public static double Factorial(double value)
        {
            long l = (long)value;
            if (l == value)
            {
                long result = 1;
                while (l > 1)
                {
                    result *= l--;
                }
                return result;
            }
            return Gamma(value + 1);
        }
        public static double Gamma(double value)
        {
            const int precision = 7;
            double[] p =
            {
                0.99999999999980993,
                676.5203681218851,
                -1259.1392167224028,
                771.32342877765313,
                -176.61502916214059,
                12.507343278686905,
                -0.13857109526572012,
                9.9843695780195716e-6,
                1.5056327351493116e-7
            };
            if (value < 0.5)
            {
                return Math.PI / Math.Sin(value * Math.PI) / Gamma(1 - value);
            }
            else
            {
                value -= 1;
                double x = p[0];
                for (int i = 1; i < precision + 2; i++)
                {
                    x += p[i] / (value + i);
                }
                double t = value + precision + 0.5;
                return Math.Sqrt(2 * Math.PI) * Math.Pow(t, (value + 0.5)) * Math.Exp(-t) * x;
            }
        }

        public static double Cot(double value)
        {
            return 1 / Math.Tan(value);
        }
        public static double Acot(double value)
        {
            return 1 / Math.Atan(value);
        }
    }
}
