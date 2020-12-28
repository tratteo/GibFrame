// Copyright (c) 2020 Matteo Beltrame

using System;
using GibFrame.Utils;
using GibFrame.Utils.Callbacks;
using GibFrame.Utils.Clocks;
using UnityEngine;

namespace GibFrame.SpawnerSystem
{
    ///<summary>
    /// Setup:
    /// <para>1. Create a Spawner passing the context (MonoBehaviour) and your spawn ranges</para>
    /// <para>2. Create a timer within the spawner calling CreateSpawnTimer</para>
    /// <para>3. Subscribe delegate method to spawn event calling SubscribeToSpawnTimerEvent</para>
    /// <para>4. Create when needed a timed spawn exception calling CreateSpawnException and either start it on creation or later calling StartTimedException</para>
    /// <para>5. Get the random spawn position in the area of the Spawner calling GetSpawnPosition, the Spawner will take care of an eventual spawn area exception</para>
    /// <para>For further info please take a look at the read_me file inside package folder</para>
    /// </summary>

    public class Spawner
    {
        public readonly Vector2 areaHorizontalRange, areaVerticalRange, areaDepthRange;
        private MonoBehaviour context;
        private bool isFixedPos = false;
        private Vector3 pos;
        private SpawnException spawnException = null;
        private Clock spawnTimer = null;

        public Spawner(MonoBehaviour context, Vector2 horizontalRange, Vector2 verticalRange, Vector2 depthRange)
        {
            isFixedPos = false;
            this.context = context;
            this.areaHorizontalRange = horizontalRange;
            this.areaVerticalRange = verticalRange;
            this.areaDepthRange = depthRange;

            if (areaHorizontalRange.x - areaHorizontalRange.y == 0
                || areaVerticalRange.x - areaVerticalRange.y == 0
                || areaDepthRange.x - areaDepthRange.y == 0)
            {
                throw new System.Exception("Spawner area is not valid, check that it is indeed a volume\n(can't be 0 of thickness in a certain direction: horizontal.x != horizontal.y for every range, vertial and depth");
            }
        }

        public Spawner(MonoBehaviour context, Vector3 pos)
        {
            this.context = context;
            isFixedPos = true;
            this.pos = pos;
        }

        /// <summary>
        ///   Create a persistent spawn Exception
        /// </summary>
        /// <param name="centre"> </param>
        /// <param name="width"> </param>
        /// <param name="height"> </param>
        /// <param name="depth"> </param>
        /// <returns> The exception instance </returns>
        public SpawnException CreateSpawnException(Vector3 centre, float width, float height, float depth)
        {
            if (spawnException != null)
            {
                spawnException.StopException();
            }

            spawnException = new SpawnException(context, this, centre, width, height, depth);
            return spawnException;
        }

        /// <summary>
        ///   Create a timed spawn Exception
        /// </summary>
        /// <param name="centre"> </param>
        /// <param name="width"> </param>
        /// <param name="height"> </param>
        /// <param name="depth"> </param>
        /// <param name="duration"> </param>
        /// <param name="startNow"> </param>
        /// <returns> The exception instance </returns>
        public SpawnException CreateSpawnException(Vector3 centre, float width, float height, float depth, float duration, bool startNow)
        {
            if (spawnException != null)
            {
                spawnException.StopException();
            }

            spawnException = new SpawnException(context, this, centre, width, height, depth, duration, startNow);
            return spawnException;
        }

        /// <summary>
        ///   Create a timer with a fixed spawn rate
        /// </summary>
        /// <param name="fixedSpawnRate"> </param>
        /// <param name="startNow"> </param>
        /// <param name="functionToSub"> </param>
        /// <returns> The Spawn Clock instance </returns>
        public Clock CreateSpawnTimer(int fixedSpawnRate, bool startNow, Action functionToSub = null)
        {
            spawnTimer = new Clock(context, fixedSpawnRate, startNow);
            SubscribeToSpawnEvent(functionToSub);
            return spawnTimer;
        }

        /// <summary>
        ///   Create a timer with a bounded spawn rate
        /// </summary>
        /// <param name="fixedSpawnRate"> </param>
        /// <param name="startNow"> </param>
        /// <param name="functionToSub"> </param>
        /// <returns> The Spawn Clock instance </returns>
        public Clock CreateSpawnTimer(Vector2 spawnRateRange, bool startNow, Action functionToSub = null)
        {
            spawnTimer = new Clock(context, spawnRateRange, startNow);
            SubscribeToSpawnEvent(functionToSub);
            return spawnTimer;
        }

        /// <summary>
        ///   Create a timer with a scale over time rate
        /// </summary>
        /// <param name="fixedSpawnRate"> </param>
        /// <param name="startNow"> </param>
        /// <param name="functionToSub"> </param>
        /// <returns> The Spawn Clock instance </returns>
        public Clock CreateSpawnTimer(Func<int, float> scaleOverTimeFunc, bool startNow, Action functionToSub = null)
        {
            spawnTimer = new Clock(context, scaleOverTimeFunc, startNow);
            SubscribeToSpawnEvent(functionToSub);
            return spawnTimer;
        }

        /// <summary>
        ///   Return an available position taking care of eventually active spawn exceptions
        /// </summary>
        /// <param name="fixedSpawnRate"> </param>
        /// <param name="startNow"> </param>
        /// <param name="functionToSub"> </param>
        /// <returns> Available position </returns>
        public Vector3 GetSpawnPosition()
        {
            if (isFixedPos)
            {
                return pos;
            }
            else
            {
                if (spawnException != null && spawnException.IsActive())
                {
                    return spawnException.GetNextPosition();
                }
                else
                {
                    return new Vector3(UnityEngine.Random.Range(areaHorizontalRange.x, areaHorizontalRange.y), UnityEngine.Random.Range(areaVerticalRange.x, areaVerticalRange.y), UnityEngine.Random.Range(areaDepthRange.x, areaDepthRange.y));
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns> Whether an excpetion is active </returns>
        public bool IsExceptionActive()
        {
            return spawnException.IsActive();
        }

        /// <summary>
        ///   Kill the spawn timer
        /// </summary>
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

        /// <summary>
        ///   Start the spawnTimer
        /// </summary>
        public void StartSpawnTimer()
        {
            spawnTimer.Start();
        }

        /// <summary>
        ///   Start a spawnException
        /// </summary>
        public void StartTimedException()
        {
            spawnException.StartException();
        }

        /// <summary>
        ///   Stop a spawnException
        /// </summary>
        public void StopException()
        {
            spawnException.StopException();
        }

        /// <summary>
        ///   Subscribe a function to be called whenever the spawnTimer fires
        /// </summary>
        /// <param name="functionToSub"> </param>
        public void SubscribeToSpawnEvent(Action functionToSub)
        {
            if (functionToSub != null)
            {
                spawnTimer.AddCallback(new Callback(functionToSub));
            }
        }

        /// <summary>
        ///   Unsubscribe a function from the spawnTimer
        /// </summary>
        /// <param name="functionToUnsub"> </param>
        public void UnsubscribeToSpawnEvent(Action functionToUnsub)
        {
            spawnTimer.RemoveCallback(new Callback(functionToUnsub));
        }
    }
}