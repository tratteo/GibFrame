//Copyright (c) matteo
//AbstractClock.cs - com.tratteo.gibframe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GibFrame.Utils.Clocks
{
    public abstract class AbstractClock
    {
        private enum RateParadigm { FIXED, RANDOM_RANGE, SCALE_FUNC }

        private RateParadigm paradigm;
        private Vector2 rateRange;
        private float currentTime;
        private bool routineActive;
        private float rate;
        private float time;
        private bool isPaused = false;
        private MonoBehaviour context;
        private Func<int, float> scaleFunc;

        protected AbstractClock(MonoBehaviour context, float fixedRate, bool startNow)
        {
            this.context = context;
            routineActive = false;
            paradigm = RateParadigm.FIXED;
            rate = fixedRate;
            if (startNow)
            {
                Start();
            }
        }

        protected AbstractClock(MonoBehaviour context, Vector2 rateRange, bool startNow)
        {
            this.context = context;
            routineActive = false;
            this.rateRange = rateRange;
            paradigm = RateParadigm.RANDOM_RANGE;
            rate = UnityEngine.Random.Range(this.rateRange.x, this.rateRange.y);
            if (startNow)
            {
                Start();
            }
        }

        protected AbstractClock(MonoBehaviour context, Func<int, float> scaleFunc, bool startNow)
        {
            this.context = context;
            routineActive = false;
            this.scaleFunc = scaleFunc;
            paradigm = RateParadigm.SCALE_FUNC;
            rate = scaleFunc(0);
            if (startNow)
            {
                Start();
            }
        }

        public void Start()
        {
            if (!routineActive)
            {
                routineActive = true;
                time = 1 / rate;
                currentTime = 0;
                context.StartCoroutine(Rate_C());
            }
        }

        public void Kill()
        {
            routineActive = false;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void EditRate(float newRate)
        {
            this.rate = newRate;
        }

        public void EditRate(Vector2 newRate)
        {
            this.rateRange = newRate;
        }

        public void EditRate(Func<int, float> newRate)
        {
            this.scaleFunc = newRate;
        }

        protected abstract void Callback();

        private IEnumerator Rate_C()
        {
            while (routineActive)
            {
                if (isPaused)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }
                if (currentTime > time)
                {
                    Callback();

                    switch (paradigm)
                    {
                        case RateParadigm.FIXED:
                            break;

                        case RateParadigm.RANDOM_RANGE:
                            rate = UnityEngine.Random.Range(rateRange.x, rateRange.y);
                            break;

                        case RateParadigm.SCALE_FUNC:
                            rate = scaleFunc((int)Time.timeSinceLevelLoad);
                            break;
                    }
                    time = 1 / rate;
                    currentTime = 0;
                }
                currentTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}