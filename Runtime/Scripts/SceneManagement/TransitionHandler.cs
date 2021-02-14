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

        public void AnimateTransition(TransitionType transition, int sceneIndex, bool async)
        {
            StartCoroutine(Animate_C(GetAnimByType(transition), sceneIndex, async));
        }

        public void AnimateTransition(TransitionType transition, string sceneName, bool async)
        {
            StartCoroutine(Animate_C(GetAnimByType(transition), SceneUtility.GetBuildIndexByScenePath(sceneName), async));
        }

        public void AnimateTransition(AnimationClip inAnim, AnimationClip outAnim, string sceneName, bool async)
        {
            custom = true;
            Transition obj = Transition.Create(inAnim, outAnim);
            obj.transform.SetParent(transform);
            StartCoroutine(Animate_C(obj, SceneUtility.GetBuildIndexByScenePath(sceneName), async));
        }

        public void AnimateTransition(string prefabName, string sceneName, bool async)
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
                    StartCoroutine(Animate_C(transition, SceneUtility.GetBuildIndexByScenePath(sceneName), async));
                }
            }
        }

        protected override void Awake()
        {
            persistent = true;
            base.Awake();
            UnityUtils.ReadGameObject(out GameObject obj, "GibFrame/SceneTransitions/Transition");
            Transition prefab = obj.GetComponent<Transition>();
            fade = Transition.Create(prefab.InAnim, prefab.OutAnim);
            fade.gameObject.name = "Fade";
            fade.transform.SetParent(transform);
        }

        private IEnumerator Animate_C(Transition transition, int sceneIndex, bool async)
        {
            GameObject anim = transition.gameObject;
            if (anim != null)
            {
                Animator animator = anim.GetComponent<Animator>();
                anim.SetActive(true);
                yield return new WaitForSeconds(transition.InDuration);
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
                yield return new WaitForSeconds(transition.OutDuration);
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