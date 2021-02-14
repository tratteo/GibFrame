// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.SceneManagement : TransitionHandler.cs
//
// All Rights Reserved

using GibFrame.Patterns;
using GibFrame.SceneManagement.Transitions;
using GibFrame.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.SceneManagement
{
    public class TransitionHandler : MonoSingleton<TransitionHandler>
    {
        public enum TransitionType { FADE }

        private Transition fade;
        private bool custom = false;

        public void AnimateTransition(TransitionType transition, int sceneIndex, bool async = false, float speed = 1F)
        {
            StartCoroutine(Animate_C(GetAnimByType(transition), sceneIndex, async, speed));
        }

        public void AnimateTransition(TransitionType transition, string sceneName, bool async = false, float speed = 1F)
        {
            StartCoroutine(Animate_C(GetAnimByType(transition), SceneUtility.GetBuildIndexByScenePath(sceneName), async, speed));
        }

        public void AnimateTransition(string prefabName, string sceneName, bool async = false, float speed = 1F)
        {
            custom = true;
            GameObject obj = UnityUtils.GetFirstChildWithName(gameObject, prefabName, true);
            if (obj == null)
            {
                throw new System.Exception("Unable to find the scene transition with the name: " + prefabName);
            }
            else
            {
                Transition transition = obj.GetComponent<Transition>();
                if (transition == null)
                {
                    throw new System.Exception("Unable to retrieve the transition component from object: " + prefabName);
                }
                else
                {
                    StartCoroutine(Animate_C(transition, SceneUtility.GetBuildIndexByScenePath(sceneName), async, speed));
                }
            }
        }

        protected override void Awake()
        {
            persistent = true;
            base.Awake();
            fade = UnityUtils.GetFirstChildWithName(gameObject, "Fade", true).GetComponent<Transition>();
        }

        private IEnumerator Animate_C(Transition transition, int sceneIndex, bool async, float speed)
        {
            GameObject anim = transition.gameObject;
            if (anim != null)
            {
                Animator animator = anim.GetComponent<Animator>();
                anim.SetActive(true);
                animator.SetFloat("Speed", speed);
                yield return new WaitForSeconds(transition.InDuration / speed);
                if (async)
                {
                    SceneUtil.LoadSceneAsynchronously(sceneIndex);
                    while (!SceneManager.GetActiveScene().buildIndex.Equals(sceneIndex))
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    SceneManager.LoadScene(sceneIndex);
                }

                animator.SetTrigger("Trigger");
                yield return new WaitForSeconds(transition.OutDuration / speed);
                anim.SetActive(false);
                if (custom)
                {
                    Destroy(transition.gameObject);
                    custom = false;
                }
            }
        }

        private Transition GetAnimByType(TransitionType transitionType)
        {
            switch (transitionType)
            {
                case TransitionType.FADE:
                    return fade;
            }
            return null;
        }
    }
}