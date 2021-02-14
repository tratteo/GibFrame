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
    }
}