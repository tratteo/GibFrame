//Copyright (c) matteo
//OnDisableAnimPerformer.cs - com.tratteo.gibframe

using System.Collections;
using UnityEngine;

namespace GibFrame
{
    public class OnDisableAnimPerformer : MonoBehaviour
    {
        [SerializeField] private string exitAnimName;
        private AnimationClip exitAnim;
        private Animator animator;

        public void SetActive(bool state)
        {
            if (state)
            {
                gameObject.SetActive(true);
            }
            else if (exitAnim != null && animator != null)
            {
                StartCoroutine(Anim_C());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if (animator.runtimeAnimatorController.animationClips[i].name == exitAnimName)
                {
                    exitAnim = animator.runtimeAnimatorController.animationClips[i];
                    break;
                }
            }
        }

        private IEnumerator Anim_C()
        {
            animator.Play(exitAnim.name);
            yield return new WaitForSeconds(exitAnim.length);
            gameObject.SetActive(false);
        }
    }
}