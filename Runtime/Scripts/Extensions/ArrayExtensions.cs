using System;

namespace GibFrame.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        ///   Copy an array to another array ignoring the elements at indexes: <paramref name="indexesExcepts"/>
        /// </summary>
        public static void CopyArrayWithExceptsAt(this Array from, Array to, int[] indexesExcepts)
        {
            Array.Sort(indexesExcepts);
            if (from.Length - to.Length != indexesExcepts.Length)
            {
                throw new System.Exception("Unable to copy arrays of wrong dimensions");
            }

            int exIndex = 0;
            int toIndex = 0;
            for (int i = 0; i < from.Length; i++)
            {
                if (i != indexesExcepts[exIndex])
                {
                    to.SetValue(from.GetValue(i), toIndex++);
                }
                else
                {
                    if (exIndex < indexesExcepts.Length - 1)
                        exIndex++;
                }
            }
        }

        /// <summary>
        ///   Copy an array to another array leaving holes at indexes: <paramref name="indexesExcepts"/>
        /// </summary>
        public static void CopyArrayWithHolesAt(this Array from, Array to, int[] indexesExcepts)
        {
            Array.Sort(indexesExcepts);
            if (to.Length - from.Length != indexesExcepts.Length)
            {
                throw new System.Exception("Unable to copy arrays of wrong dimensions");
            }

            int elementToCopy = 0;
            int exIndex = 0;

            for (int i = 0; i < to.Length; i++)
            {
                if (exIndex > indexesExcepts.Length - 1)
                {
                    to.SetValue(from.GetValue(elementToCopy++), i);
                }
                else if (i != indexesExcepts[exIndex])
                {
                    to.SetValue(from.GetValue(elementToCopy++), i);
                }
                else
                {
                    exIndex++;
                }
            }
        }
    }
}
