// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.SpawnerSystem : SpawnException2D.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame.SpawnerSystem
{
    public class SpawnException2D : MonoBehaviour
    {
        private enum ExceptionType { ZERO_IN_A_DIRECTION, OUT_OF_SAREA, WHOLE_SAREA, NO_EX }

        private Vector2 horizontalBound, verticalBound;
        private Vector2 centre;
        private float duration;
        private bool isActive;
        private MonoBehaviour context;
        private Spawner2D spawner;
        private float[] sectionsProbability;

        public SpawnException2D(MonoBehaviour context, Spawner2D spawner, Vector2 centre, float width, float height, float duration, bool startNow)
        {
            this.context = context;
            this.centre = centre;
            this.spawner = spawner;
            this.duration = duration;

            CheckException(width, height);
            if (startNow)
                StartException();
        }

        public SpawnException2D(MonoBehaviour context, Spawner2D spawner, Vector2 centre, float width, float height)
        {
            this.context = context;
            this.centre = centre;
            this.spawner = spawner;

            CheckException(width, height);
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

        public Vector2 GetNextPosition()
        {
            Vector2 point = new Vector2(0, 0);

            int index = RandomIndex(sectionsProbability);

            switch (index)
            {
                case 0:
                    point.x = Random.Range(spawner.areaHorizontalRange.x, horizontalBound.x);
                    point.y = Random.Range(spawner.areaVerticalRange.x, spawner.areaVerticalRange.y);
                    break;

                case 1:
                    point.x = Random.Range(horizontalBound.x, horizontalBound.y);
                    point.y = Random.Range(verticalBound.y, spawner.areaVerticalRange.y);
                    break;

                case 2:
                    point.x = Random.Range(horizontalBound.x, horizontalBound.y);
                    point.y = Random.Range(spawner.areaVerticalRange.x, verticalBound.x);
                    break;

                case 3:
                    point.x = Random.Range(horizontalBound.y, spawner.areaHorizontalRange.y);
                    point.y = Random.Range(spawner.areaVerticalRange.x, spawner.areaVerticalRange.y);
                    break;

                case 4:
                    point.x = Random.Range(horizontalBound.x, horizontalBound.y);
                    point.y = Random.Range(verticalBound.x, verticalBound.y);
                    break;

                case 5:
                    point.x = Random.Range(horizontalBound.x, horizontalBound.y);
                    point.y = Random.Range(verticalBound.x, verticalBound.y);
                    break;
            }
            return point;
        }

        private void CheckException(float width, float height)
        {
            ExceptionType ex = InitializeSpawnException(width, height);
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

        private ExceptionType InitializeSpawnException(float exceptionWidth, float exceptionHeight)
        {
            if (exceptionWidth == 0 || exceptionHeight == 0)
            {
                return ExceptionType.ZERO_IN_A_DIRECTION;
            }
            exceptionWidth = Mathf.Abs(exceptionWidth);
            exceptionHeight = Mathf.Abs(exceptionHeight);

            horizontalBound.x = centre.x - (exceptionWidth);
            horizontalBound.y = centre.x + (exceptionWidth);
            verticalBound.x = centre.y - (exceptionHeight);
            verticalBound.y = centre.y + (exceptionHeight);

            horizontalBound.x = horizontalBound.x < spawner.areaHorizontalRange.x ? spawner.areaHorizontalRange.x : horizontalBound.x;
            horizontalBound.y = horizontalBound.y > spawner.areaHorizontalRange.y ? spawner.areaHorizontalRange.y : horizontalBound.y;

            verticalBound.x = verticalBound.x < spawner.areaVerticalRange.x ? spawner.areaVerticalRange.x : verticalBound.x;
            verticalBound.y = verticalBound.y > spawner.areaVerticalRange.y ? spawner.areaVerticalRange.y : verticalBound.y;

            if (horizontalBound.x > spawner.areaHorizontalRange.y ||
               horizontalBound.y < spawner.areaHorizontalRange.x ||
               verticalBound.x > spawner.areaVerticalRange.y ||
               verticalBound.y < spawner.areaVerticalRange.x)
            {
                return ExceptionType.OUT_OF_SAREA;
            }

            if (horizontalBound.x == spawner.areaHorizontalRange.x && horizontalBound.y == spawner.areaHorizontalRange.y &&
               verticalBound.x == spawner.areaVerticalRange.x && verticalBound.y == spawner.areaVerticalRange.y)
            {
                return ExceptionType.WHOLE_SAREA;
            }
            sectionsProbability = new float[6];
            sectionsProbability[0] = (horizontalBound.x - spawner.areaHorizontalRange.x) * (spawner.areaVerticalRange.y - spawner.areaVerticalRange.x);
            sectionsProbability[1] = (horizontalBound.y - horizontalBound.x) * (spawner.areaVerticalRange.y - verticalBound.y);
            sectionsProbability[2] = (horizontalBound.y - horizontalBound.x) * (verticalBound.x - spawner.areaVerticalRange.x);
            sectionsProbability[3] = (spawner.areaHorizontalRange.y - horizontalBound.y) * (spawner.areaVerticalRange.y - spawner.areaVerticalRange.x);
            sectionsProbability[4] = (horizontalBound.y - horizontalBound.x) * (verticalBound.y - verticalBound.x);
            sectionsProbability[5] = (horizontalBound.y - horizontalBound.x) * (verticalBound.y - verticalBound.x);
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