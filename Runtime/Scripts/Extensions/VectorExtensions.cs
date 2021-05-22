//Copyright (c) matteo
//VectorExtensions.cs - com.tratteo.gibframe

using UnityEngine;

namespace GibFrame
{
    public static class VectorExtensions
    {
        #region Vector3

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
            return source + (axis * GMath.Map((float)new System.Random(seed).NextDouble(), (0F, 1F), (-magnitude, magnitude)));
        }

        public static Vector3 Perturbate(this Vector3 source, int seed, float magnitude = 1F)
        {
            System.Random rand = new System.Random(seed);
            float x = GMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            float y = GMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            float z = GMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            return source + new Vector3(x, y, z);
        }

        public static Vector3 Perturbate(this Vector3 source, Vector3 axis, System.Random random, float magnitude = 1F)
        {
            return source + (axis * GMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude)));
        }

        public static Vector3 Perturbate(this Vector3 source, System.Random random, float magnitude = 1F)
        {
            float x = GMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            float y = GMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            float z = GMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
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
            return source + (axis * GMath.Map((float)new System.Random(seed).NextDouble(), (0F, 1F), (-magnitude, magnitude)));
        }

        public static Vector2 Perturbate(this Vector2 source, int seed, float magnitude = 1F)
        {
            System.Random rand = new System.Random(seed);
            float x = GMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            float y = GMath.Map((float)rand.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            return source + new Vector2(x, y);
        }

        public static Vector2 Perturbate(this Vector2 source, Vector2 axis, System.Random random, float magnitude = 1F)
        {
            return source + (axis * GMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude)));
        }

        public static Vector2 Perturbate(this Vector2 source, System.Random random, float magnitude = 1F)
        {
            float x = GMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            float y = GMath.Map((float)random.NextDouble(), (0F, 1F), (-magnitude, magnitude));
            return source + new Vector2(x, y);
        }

        #endregion Vector2
    }
}