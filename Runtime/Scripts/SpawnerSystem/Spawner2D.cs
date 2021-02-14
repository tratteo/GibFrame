// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.SpawnerSystem : Spawner2D.cs
//
// All Rights Reserved

using System;
using GibFrame.Utils.Callbacks;
using GibFrame.Utils.Clocks;
using UnityEngine;

namespace GibFrame.SpawnerSystem
{
    public class Spawner2D : MonoBehaviour
    {
        public readonly Vector2 areaHorizontalRange, areaVerticalRange;
        private Clock spawnTimer = null;
        private SpawnException2D spawnException = null;
        private MonoBehaviour context;

        public Spawner2D(MonoBehaviour context, Vector2 horizontalRange, Vector2 verticalRange)
        {
            this.context = context;
            this.areaHorizontalRange = horizontalRange;
            this.areaVerticalRange = verticalRange;

            if (areaHorizontalRange.x - areaHorizontalRange.y == 0
                || areaVerticalRange.x - areaVerticalRange.y == 0)
            {
                throw new System.Exception("Spawner area is not valid, check that it is indeed a volume\n(can't be 0 of thickness in a certain direction: horizontal.x != horizontal.y for every range, vertial and depth");
            }
        }

        public Clock CreateSpawnTimer(int fixedSpawnRate, bool startNow)
        {
            spawnTimer = new Clock(context, fixedSpawnRate, startNow);
            return spawnTimer;
        }

        public Clock CreateSpawnTimer(Vector2 spawnRateRange, bool startNow)
        {
            spawnTimer = new Clock(context, spawnRateRange, startNow);
            return spawnTimer;
        }

        public Clock CreateSpawnTimer(Func<int, float> scaleOverTimeFunc, bool startNow)
        {
            spawnTimer = new Clock(context, scaleOverTimeFunc, startNow);
            return spawnTimer;
        }

        public void StartSpawnTimer()
        {
            spawnTimer.Start();
        }

        public void KillSpawnTimer()
        {
            spawnTimer.Kill();
        }

        /// <summary>
        ///   Pause the spawnTimer
        /// </summary>
        public void PauseSpawnTimer()
        {
            spawnTimer.Pause();
        }

        /// <summary>
        ///   Resume the spawnTimer
        /// </summary>
        public void ResumeSpawnTimer()
        {
            spawnTimer.Resume();
        }

        public void SubscribeToSpawnEvent(Action functionToSub)
        {
            spawnTimer.AddCallback(new Callback(functionToSub));
        }

        public void UnsubscribeToSpawnEvent(Action functionToUnsub)
        {
            spawnTimer.RemoveCallback(new Callback(functionToUnsub));
        }

        public SpawnException2D CreateSpawnException(Vector3 centre, float width, float height)
        {
            if (spawnException != null)
            {
                spawnException.StopException();
            }

            spawnException = new SpawnException2D(context, this, centre, width, height);
            return spawnException;
        }

        public SpawnException2D CreateSpawnException(Vector3 centre, float width, float height, float duration, bool startNow)
        {
            if (spawnException != null)
            {
                spawnException.StopException();
            }

            spawnException = new SpawnException2D(context, this, centre, width, height, duration, startNow);
            return spawnException;
        }

        public void StartTimedException()
        {
            spawnException.StartException();
        }

        public void StopException()
        {
            spawnException.StopException();
        }

        public bool IsExceptionActive()
        {
            return spawnException.IsActive();
        }

        public Vector2 GetSpawnPosition()
        {
            if (spawnException != null && spawnException.IsActive())
            {
                return spawnException.GetNextPosition();
            }
            else
            {
                return new Vector2(UnityEngine.Random.Range(areaHorizontalRange.x, areaHorizontalRange.y), UnityEngine.Random.Range(areaVerticalRange.x, areaVerticalRange.y));
            }
        }
    }
}