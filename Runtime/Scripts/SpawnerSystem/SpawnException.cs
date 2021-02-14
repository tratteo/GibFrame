// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.SpawnerSystem : SpawnException.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame.SpawnerSystem
{
    public class SpawnException
    {
        private enum ExceptionType { ZERO_IN_A_DIRECTION, OUT_OF_SAREA, WHOLE_SAREA, NO_EX }

        private Vector2 horizontalBound, verticalBound, depthBound;
        private Vector3 centre;
        private float duration;
        private bool isActive;
        private MonoBehaviour context;
        private Spawner spawner;
        private float[] sectionsProbability;

        public SpawnException(MonoBehaviour context, Spawner spawner, Vector3 centre, float width, float height, float depth, float duration, bool startNow)
        {
            this.context = context;
            this.centre = centre;
            this.spawner = spawner;
            this.duration = duration;

            CheckException(width, height, depth);
            if (startNow)
                StartException();
        }

        public SpawnException(MonoBehaviour context, Spawner spawner, Vector3 centre, float width, float height, float depth)
        {
            this.context = context;
            this.centre = centre;
            this.spawner = spawner;

            CheckException(width, height, depth);
            isActive = true;
        }

        public void StartException()
        {
            if (context != null && !isActive)
            {
                isActive = true;
                context.StartCoroutine(ExceptionCoroutine());
            }
        }

        public void StopException()
        {
            isActive = false;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public Vector3 GetNextPosition()
        {
            Vector3 point = new Vector3(0, 0, 0);

            int index = RandomIndex(sectionsProbability);

            switch (index)
            {
                case 0:
                    point.x = Random.Range(spawner.areaHorizontalRange.x, horizontalBound.x);
                    point.y = Random.Range(spawner.areaVerticalRange.x, spawner.areaVerticalRange.y);
                    point.z = Random.Range(spawner.areaDepthRange.x, spawner.areaDepthRange.y);
                    break;

                case 1:
                    point.x = Random.Range(horizontalBound.x, horizontalBound.y);
                    point.y = Random.Range(verticalBound.y, spawner.areaVerticalRange.y);
                    point.z = Random.Range(spawner.areaDepthRange.x, spawner.areaDepthRange.y);
                    break;

                case 2:
                    point.x = Random.Range(horizontalBound.x, horizontalBound.y);
                    point.y = Random.Range(spawner.areaVerticalRange.x, verticalBound.x);
                    point.z = Random.Range(spawner.areaDepthRange.x, spawner.areaDepthRange.y);
                    break;

                case 3:
                    point.x = Random.Range(horizontalBound.y, spawner.areaHorizontalRange.y);
                    point.y = Random.Range(spawner.areaVerticalRange.x, spawner.areaVerticalRange.y);
                    point.z = Random.Range(spawner.areaDepthRange.x, spawner.areaDepthRange.y);
                    break;

                case 4:
                    point.x = Random.Range(horizontalBound.x, horizontalBound.y);
                    point.y = Random.Range(verticalBound.x, verticalBound.y);
                    point.z = Random.Range(spawner.areaDepthRange.x, depthBound.x);
                    break;

                case 5:
                    point.x = Random.Range(horizontalBound.x, horizontalBound.y);
                    point.y = Random.Range(verticalBound.x, verticalBound.y);
                    point.z = Random.Range(depthBound.y, spawner.areaDepthRange.y);
                    break;
            }
            return point;
        }

        private void CheckException(float width, float height, float depth)
        {
            ExceptionType ex = InitializeSpawnException(width, height, depth);
            switch (ex)
            {
                case ExceptionType.ZERO_IN_A_DIRECTION:
                    throw new System.Exception("SpawnException has 0 in a direction");
                case ExceptionType.OUT_OF_SAREA:
                    throw new System.Exception("SpawnException is completely out of the SpawnArea");
                case ExceptionType.WHOLE_SAREA:
                    throw new System.Exception("SpawnException takes the whole SpawnArea");
            }
        }

        private ExceptionType InitializeSpawnException(float exceptionWidth, float exceptionHeight, float exceptionDepth)
        {
            if (exceptionWidth == 0 || exceptionHeight == 0 || exceptionDepth == 0)
            {
                return ExceptionType.ZERO_IN_A_DIRECTION;
            }
            exceptionWidth = Mathf.Abs(exceptionWidth);
            exceptionHeight = Mathf.Abs(exceptionHeight);
            exceptionDepth = Mathf.Abs(exceptionDepth);

            horizontalBound.x = centre.x - (exceptionWidth);
            horizontalBound.y = centre.x + (exceptionWidth);
            verticalBound.x = centre.y - (exceptionHeight);
            verticalBound.y = centre.y + (exceptionHeight);
            depthBound.x = centre.z - (exceptionDepth);
            depthBound.y = centre.z + (exceptionDepth);

            horizontalBound.x = horizontalBound.x < spawner.areaHorizontalRange.x ? spawner.areaHorizontalRange.x : horizontalBound.x;
            horizontalBound.y = horizontalBound.y > spawner.areaHorizontalRange.y ? spawner.areaHorizontalRange.y : horizontalBound.y;

            verticalBound.x = verticalBound.x < spawner.areaVerticalRange.x ? spawner.areaVerticalRange.x : verticalBound.x;
            verticalBound.y = verticalBound.y > spawner.areaVerticalRange.y ? spawner.areaVerticalRange.y : verticalBound.y;

            depthBound.x = depthBound.x < spawner.areaDepthRange.x ? spawner.areaDepthRange.x : depthBound.x;
            depthBound.y = depthBound.y > spawner.areaDepthRange.y ? spawner.areaDepthRange.y : depthBound.y;

            if (horizontalBound.x > spawner.areaHorizontalRange.y ||
               horizontalBound.y < spawner.areaHorizontalRange.x ||
               verticalBound.x > spawner.areaVerticalRange.y ||
               verticalBound.y < spawner.areaVerticalRange.x ||
               depthBound.x > spawner.areaDepthRange.y ||
               depthBound.y < spawner.areaDepthRange.x)
            {
                return ExceptionType.OUT_OF_SAREA;
            }

            if (horizontalBound.x == spawner.areaHorizontalRange.x && horizontalBound.y == spawner.areaHorizontalRange.y &&
               verticalBound.x == spawner.areaVerticalRange.x && verticalBound.y == spawner.areaVerticalRange.y &&
               depthBound.x == spawner.areaDepthRange.x && depthBound.y == spawner.areaDepthRange.y)
            {
                return ExceptionType.WHOLE_SAREA;
            }
            sectionsProbability = new float[6];
            sectionsProbability[0] = (horizontalBound.x - spawner.areaHorizontalRange.x) * (spawner.areaVerticalRange.y - spawner.areaVerticalRange.x) * (spawner.areaDepthRange.y - spawner.areaDepthRange.x);
            sectionsProbability[1] = (horizontalBound.y - horizontalBound.x) * (spawner.areaVerticalRange.y - verticalBound.y) * (spawner.areaDepthRange.y - spawner.areaDepthRange.x);
            sectionsProbability[2] = (horizontalBound.y - horizontalBound.x) * (verticalBound.x - spawner.areaVerticalRange.x) * (spawner.areaDepthRange.y - spawner.areaDepthRange.x);
            sectionsProbability[3] = (spawner.areaHorizontalRange.y - horizontalBound.y) * (spawner.areaVerticalRange.y - spawner.areaVerticalRange.x) * (spawner.areaDepthRange.y - spawner.areaDepthRange.x);
            sectionsProbability[4] = (horizontalBound.y - horizontalBound.x) * (verticalBound.y - verticalBound.x) * (depthBound.x - spawner.areaDepthRange.x);
            sectionsProbability[5] = (horizontalBound.y - horizontalBound.x) * (verticalBound.y - verticalBound.x) * (spawner.areaDepthRange.y - depthBound.y);
            NormalizeProbability(sectionsProbability);

            return ExceptionType.NO_EX;
        }

        private IEnumerator ExceptionCoroutine()
        {
            yield return new WaitForSeconds(duration);
            isActive = false;
        }

        private int RandomIndex(float[] probs)
        {
            int n = probs.Length;
            int index = -1;
            float radix = UnityEngine.Random.Range(0f, 1f);
            while (radix > 0 && index < n)
            {
                radix -= probs[++index];
            }
            return index;
        }

        private void NormalizeProbability(float[] probs)
        {
            int n = probs.Length;
            float tot = 0;
            for (int i = 0; i < n; i++)
            {
                tot += probs[i];
            }
            for (int i = 0; i < n; i++)
            {
                probs[i] /= tot;
            }
        }
    }
}