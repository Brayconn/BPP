using System;

namespace BrayconnsPatchingFramework
{
    static class Extensions
    {
        /// <summary>
        /// Clones an object[] by casting each element to an IClonable
        /// </summary>
        /// <param name="arr">The array</param>
        /// <returns>A deep copy of the array</returns>
        public static object[] DeepClone(this object[] arr)
        {
            return UnsafeDeepClone(arr);
        }

        /// <summary>
        /// Clones an array of IClonables
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="arr">The array</param>
        /// <returns>A deep copy of the array</returns>
        public static T[] DeepClone<T>(this T[] arr) where T : class, ICloneable
        {
            return UnsafeDeepClone(arr);
        }

        //trying to be nice by keeping this version private
        //since the others have actual restrictions to stop you from doing dumb stuff quite as easily
        private static T[] UnsafeDeepClone<T>(T[] ogArr) where T : class
        {
            T[] newArr = new T[ogArr.Length];
            for (int i = 0; i < ogArr.Length; i++)
                newArr[i] = (T)((ICloneable)ogArr[i]).Clone();
            return newArr;
        }
    }
}
