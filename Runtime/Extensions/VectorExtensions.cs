// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : VectorExtensions.cs
//
// All Rights Reserved

using UnityEngine;

namespace GibFrame.Extensions
{
    public static class VectorExtensions
    {
        #region Vector3

        public static Vector3 AsPositive(this Vector3 source)
        {
            var res = source;
            res.Positivize();
            return res;
        }

        public static void Positivize(this ref Vector3 source)
        {
            source.x = Mathf.Abs(source.x);
            source.y = Mathf.Abs(source.y);
            source.z = Mathf.Abs(source.z);
        }

        public static Vector3 AsNegative(this Vector3 source)
        {
            var res = source;
            res.Negativize();
            return res;
        }

        public static void Negativize(this ref Vector3 source)
        {
            source.x = -Mathf.Abs(source.x);
            source.y = -Mathf.Abs(source.y);
            source.z = -Mathf.Abs(source.z);
        }

        public static Vector3 Perturbate(this Vector3 source, Vector3 axis, float magnitude)
        {
            return source + (axis * UnityEngine.Random.Range(-magnitude, magnitude));
        }

        public static Vector3 Perturbate(this Vector3 source, float magnitude = 1F)
        {
            return source + new Vector3(UnityEngine.Random.Range(-magnitude, magnitude), UnityEngine.Random.Range(-magnitude, magnitude), UnityEngine.Random.Range(-magnitude, magnitude));
        }

        public static Vector3 Perturbate(this Vector3 source, Vector3 axis, float magnitude, int seed)
        {
            return source + (axis * GibMath.Map((float)new System.Random(seed).NextDouble(), (0F, 1F), (-magnitude, magnitude)));
        }

        public static Vector3 Perturbate(this Vector3 source, int seed, float magnitude = 1F)
        {
            var rand = new System.Random(seed);
            var x = GibMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            var y = GibMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            var z = GibMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            return source + new Vector3(x, y, z);
        }

        public static Vector3 Perturbate(this Vector3 source, Vector3 axis, System.Random random, float magnitude = 1F)
        {
            return source + (axis * GibMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude)));
        }

        public static Vector3 Perturbate(this Vector3 source, System.Random random, float magnitude = 1F)
        {
            var x = GibMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            var y = GibMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            var z = GibMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            return source + new Vector3(x, y, z);
        }

        #endregion Vector3

        #region Vector2

        public static Vector2 Perturbate(this Vector2 source, Vector2 axis, float magnitude)
        {
            return source + (axis * UnityEngine.Random.Range(-magnitude, magnitude));
        }

        public static Vector2 Perturbate(this Vector2 source, float magnitude = 1F)
        {
            return source + new Vector2(UnityEngine.Random.Range(-magnitude, magnitude), UnityEngine.Random.Range(-magnitude, magnitude));
        }

        public static Vector2 Perturbate(this Vector2 source, Vector2 axis, float magnitude, int seed)
        {
            return source + (axis * GibMath.Map((float)new System.Random(seed).NextDouble(), (0F, 1F), (-magnitude, magnitude)));
        }

        public static Vector2 Perturbate(this Vector2 source, int seed, float magnitude = 1F)
        {
            var rand = new System.Random(seed);
            var x = GibMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            var y = GibMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            return source + new Vector2(x, y);
        }

        public static Vector2 Perturbate(this Vector2 source, Vector2 axis, System.Random random, float magnitude = 1F)
        {
            return source + (axis * GibMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude)));
        }

        public static Vector2 Perturbate(this Vector2 source, System.Random random, float magnitude = 1F)
        {
            var x = GibMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            var y = GibMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            return source + new Vector2(x, y);
        }

        public static void Positivize(this ref Vector2 source)
        {
            source.x = Mathf.Abs(source.x);
            source.y = Mathf.Abs(source.y);
        }

        public static Vector2 AsPositive(this Vector2 source)
        {
            var res = source;
            res.Positivize();
            return res;
        }

        public static Vector2 AsNegative(this Vector2 source)
        {
            var res = source;
            res.Negativize();
            return res;
        }

        public static void Negativize(this ref Vector2 source)
        {
            source.x = -Mathf.Abs(source.x);
            source.y = -Mathf.Abs(source.y);
        }

        #endregion Vector2
    }
}