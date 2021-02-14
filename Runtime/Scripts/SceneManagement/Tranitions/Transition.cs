using GibFrame.Utils;
using UnityEngine;

namespace GibFrame.SceneManagement.Transitions
{
    public class Transition : MonoBehaviour
    {
        [SerializeField] private AnimationClip inAnim;
        [SerializeField] private AnimationClip outAnim;

        public float InDuration => inAnim.length;

        public AnimationClip InAnim => inAnim;

        public AnimationClip OutAnim => outAnim;

        public float OutDuration => outAnim.length;

        internal static Transition Create(AnimationClip inAnim, AnimationClip outAnim)
        {
            UnityUtils.ReadGameObject(out GameObject prefab, "GibFrame/SceneTransitions/Transition");
            GameObject obj = Instantiate(prefab);
            Transition transition = obj.GetComponent<Transition>();
            transition.inAnim = inAnim;
            transition.outAnim = outAnim;
            return transition;
        }
    }
}