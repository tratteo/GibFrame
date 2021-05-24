// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.SceneManagement : TransitionHandler.cs
//
// All Rights Reserved

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.SceneManagement
{
    public class TransitionHandler : MonoSingleton<TransitionHandler>
    {
        [SerializeField] private string inAnimationName = "In";
        [SerializeField] private string outAnimationName = "Out";

        public bool IsTransitioning { get; private set; } = false;

        public void AnimateTransition(string childName, string sceneName, bool async = false, float speed = 1F)
        {
            Transform obj = transform.GetFirstChildWithName(childName, true);
            if (obj == null)
            {
                UnityEngine.Debug.LogError("TransitionHandler: unable to find the scene transition with the name: " + childName);
            }
            else
            {
                Animator animator = obj.GetComponent<Animator>();
                if (animator == null)
                {
                    UnityEngine.Debug.LogError("TransitionHandler: unable to retrieve the animator component from object: " + childName);
                }
                else
                {
                    StartCoroutine(Animate_C(animator, SceneUtility.GetBuildIndexByScenePath(sceneName), async, speed));
                }
            }
        }

        protected override void Awake()
        {
            persistent = true;
            base.Awake();
        }

        private IEnumerator Animate_C(Animator transitionAnimator, int sceneIndex, bool async, float speed)
        {
            if (!IsTransitioning && transitionAnimator)
            {
                AnimationClip inClip = transitionAnimator.runtimeAnimatorController.animationClips.ToList().Find(c => c.name.Equals(inAnimationName));
                AnimationClip outClip = transitionAnimator.runtimeAnimatorController.animationClips.ToList().Find(c => c.name.Equals(outAnimationName));
                if (inClip == null || outClip == null)
                {
                    UnityEngine.Debug.LogError("Unable to retrieve the animations from the transition animator, double check that the names are correct");
                }
                else
                {
                    IsTransitioning = true;
                    transitionAnimator.gameObject.SetActive(true);
                    transitionAnimator.speed = speed;
                    transitionAnimator.Play(inClip.name);
                    yield return new WaitForSeconds(inClip.length / speed);
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

                    transitionAnimator.Play(outClip.name);
                    yield return new WaitForSeconds(outClip.length / speed);
                    transitionAnimator.gameObject.SetActive(false);
                    IsTransitioning = false;
                }
            }
        }
    }
}
