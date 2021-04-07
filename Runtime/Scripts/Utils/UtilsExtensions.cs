using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibFrame.Utils
{
    public static class UtilsExtensions
    {
        /// <summary>
        ///   Select an item based on its probability (interface)
        /// </summary>
        /// <param name="set"> </param>
        /// <returns> The selecte item </returns>
        public static T SelectWithProbability<T>(this List<T> set) where T : IProbSelectable
        {
            int index = -1;
            float r = UnityEngine.Random.Range(0F, 1F);
            while (r > 0)
            {
                r -= set.ElementAt(++index).ProvideSelectProbability();
            }
            return (T)set.ElementAt(index);
        }

        public static T SelectWithProbability<T>(this List<T> set, System.Random random) where T : IProbSelectable
        {
            int index = -1;
            float r = (float)random.NextDouble();
            while (r > 0)
            {
                r -= set.ElementAt(++index).ProvideSelectProbability();
            }
            return (T)set.ElementAt(index);
        }

        /// <summary>
        ///   Normalize the select probabilities using the <b> float value(T:IProbSelectable) </b> function
        /// </summary>
        /// <param name="set"> </param>
        /// <returns> </returns>
        public static void NormalizeProbabilities<T>(this List<T> set, Func<T, float> value) where T : IProbSelectable
        {
            float sum = 0;
            foreach (T elem in set)
            {
                sum += value(elem);
            }
            if (sum == 0)
            {
                foreach (T elem1 in set)
                {
                    elem1.SetSelectProbability(1F / set.Count);
                }
            }
            else
            {
                foreach (T elem1 in set)
                {
                    elem1.SetSelectProbability(value(elem1) / sum);
                }
            }
        }

        public static void NormalizeProbabilities<T>(this List<T> selectables) where T : IProbSelectable
        {
            NormalizeProbabilities(selectables, (s) => s.ProvideSelectProbability());
        }

        public static void NormalizeProbabilities<T>(this T[] selectables) where T : IProbSelectable
        {
            NormalizeProbabilities(selectables.ToList());
        }
    }
}
