//Copyright (c) matteo
//GMath.cs - com.tratteo.gibframe

using System;

namespace GibFrame
{
    /// <summary>
    ///   Trat Math
    /// </summary>
    public static class GMath
    {
        public const float RadToDeg = 57.29578F;
        public const float DegToRad = 0.0174533F;

        public static float Sinh(float x)
        {
            return (float)(Math.Exp(x) - Math.Exp(-x)) / 2F;
        }

        public static double Tanh(double x)
        {
            double exp = (double)Math.Exp(2 * x);
            return (exp - 1) / (exp + 1);
        }

        public static float Cosh(float x)
        {
            return (float)(Math.Exp(x) + Math.Exp(-x)) / 2F;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"> </param>
        /// <param name="y"> </param>
        /// <returns> Arctan value given the 2 catetis </returns>
        public static float Arctan4(float x, float y)
        {
            if (x >= 0)
            {
                if (y >= 0)
                {
                    if (y == 0) return -90f;

                    return (float)-Math.Atan(x / y) * GMath.RadToDeg;
                }
                else
                {
                    return -180f + ((float)Math.Atan(x / -y) * GMath.RadToDeg);
                }
            }
            else
            {
                if (y >= 0)
                {
                    if (y == 0) return 90f;

                    return (float)Math.Atan(-x / y) * GMath.RadToDeg;
                }
                else
                {
                    return 180f - ((float)Math.Atan(x / y) * GMath.RadToDeg);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="x"> </param>
        /// <param name="a"> </param>
        /// <param name="b"> </param>
        /// <param name="c"> </param>
        /// <returns> The Gaussian function with a, b and c costants </returns>
        public static float Gaussian(float x, float a, float b, float c)
        {
            if (c == 0)
            {
                throw new System.Exception("TMath Exception -> Costant C cannot be 0 in Gaussian function");
            }
            float sub = x - b;
            return (float)(a * Math.Exp(-(sub * sub) / (c * c)));
        }

        /// <summary>
        /// </summary>
        /// <param name="a"> </param>
        /// <param name="b"> </param>
        /// <param name="x"> </param>
        /// <returns> e function raised to the b*x power and multiplied by a </returns>
        public static double AdjExp(float a, float b, float x)
        {
            return Math.Exp(b * x) * a;
        }

        public static int KroneckerDelta(int i, int j)
        {
            return (i == j ? 1 : 0);
        }

        public static int HeavisideStep(int x)
        {
            return (x > 0 ? 1 : 0);
        }

        public static int LeviCivitaTensor(int i, int j, int k)
        {
            return Math.Sign(j - i) + Math.Sign(k - j) + Math.Sign(i - k);
        }

        public static float Abs(float value)
        {
            return value > 0 ? value : -value;
        }

        public static int Abs(int value)
        {
            return value > 0 ? value : -value;
        }

        /// <summary>
        /// </summary>
        /// <param name="data"> </param>
        /// <returns> The max value between an arbitrary number of values </returns>
        public static int Max(params int[] data)
        {
            int max = int.MinValue;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > max)
                {
                    max = data[i];
                }
            }
            return max;
        }

        public static float Max(params float[] data)
        {
            float max = float.MinValue;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > max)
                {
                    max = data[i];
                }
            }
            return max;
        }

        public static double Max(params double[] data)
        {
            double max = double.MinValue;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > max)
                {
                    max = data[i];
                }
            }
            return max;
        }

        /// <summary>
        /// </summary>
        /// <param name="data"> </param>
        /// <returns> The min value between an arbitrary number of values </returns>
        public static int Min(params int[] data)
        {
            int min = int.MaxValue;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < min)
                {
                    min = data[i];
                }
            }
            return min;
        }

        public static float Min(params float[] data)
        {
            float min = float.MaxValue;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < min)
                {
                    min = data[i];
                }
            }
            return min;
        }

        public static double Min(params double[] data)
        {
            double min = double.MaxValue;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < min)
                {
                    min = data[i];
                }
            }
            return min;
        }

        /// <summary>
        /// </summary>
        /// <param name="data"> </param>
        /// <returns> The average of an arbitrary number of values </returns>
        public static float Avg(params float[] data)
        {
            float sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum / data.Length;
        }

        public static double Avg(params double[] data)
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum / data.Length;
        }

        public static float Avg(params int[] data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum / data.Length;
        }

        /// <summary>
        /// </summary>
        /// <param name="data"> </param>
        /// <returns> Te sum of an arbitrary number of values </returns>
        public static double Sum(params double[] data)
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        public static float Sum(params float[] data)
        {
            float sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        public static int Sum(params int[] data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        public static float Map(float val, (float, float) range, (float, float) newRange)
        {
            return (val - range.Item1) / (range.Item2 - range.Item1) * (newRange.Item2 - newRange.Item1) + newRange.Item1;
        }

        public static int RoundTo(int val, int seed = 2)
        {
            return (int)Math.Round((float)val / seed) * seed;
        }

        public static int CeilTo(int val, int seed = 2)
        {
            return (int)Math.Ceiling((float)val / seed) * seed;
        }

        public static int FloorTo(int val, int seed = 2)
        {
            return (int)Math.Floor((float)val / seed) * seed;
        }

        public static float RoundTo(float val, float seed = 1F)
        {
            return (float)Math.Round(val / seed) * seed;
        }

        public static float CeilTo(float val, float seed = 1F)
        {
            return (float)Math.Ceiling(val / seed) * seed;
        }

        public static float FloorTo(float val, float seed = 1F)
        {
            return (float)Math.Floor(val / seed) * seed;
        }

        public static class RandomGen
        {
            private static bool nextGaussianAvailable;
            private static double nextGaussian;

            /// <summary>
            /// </summary>
            /// <param name="mean"> </param>
            /// <param name="stdv"> </param>
            /// <returns> A random Gaussian distributed number </returns>
            public static double NextGaussian(double mean, double stdv)
            {
                double amplitude = 1F / Math.Sqrt(2F * Math.PI * stdv * stdv);
                if (nextGaussianAvailable)
                {
                    nextGaussianAvailable = false;
                    return nextGaussian;
                }
                else
                {
                    double u1 = UnityEngine.Random.Range(0F, 1F);
                    double u2 = UnityEngine.Random.Range(0F, 1F);
                    double x = 0;
                    double y = 0;
                    if (u1 != 0)
                    {
                        x = amplitude * (mean + stdv * (Math.Sqrt(-2F * Math.Log(u1))) * Math.Cos(2F * Math.PI * u2));
                    }
                    if (u2 != 0)
                    {
                        y = amplitude * (mean + stdv * (Math.Sqrt(-2F * Math.Log(u1))) * Math.Sin(2F * Math.PI * u2));
                    }
                    nextGaussianAvailable = true;
                    nextGaussian = y;
                    return x;
                }
            }
        }
    }
}