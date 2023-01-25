using GibFrame.Extensions;
using UnityEngine;

namespace GibFrame
{
    [ExecuteAlways]
    public class ScaleRelator : MonoBehaviour
    {
        [SerializeField] private bool disjoinX;
        [SerializeField] private bool disjoinY;
        [SerializeField] private bool disjoinZ;
        [SerializeField] private Transform rootTarget;

        private void Start()
        {
            if (!rootTarget) rootTarget = transform.root;
        }

        private void LateUpdate()
        {
            var currentScale = transform.localScale.AsPositive();
            var signX = disjoinX ? Mathf.Sign(rootTarget.localScale.x) : 1F;
            var signY = disjoinY ? Mathf.Sign(rootTarget.localScale.y) : 1F;
            var signZ = disjoinZ ? Mathf.Sign(rootTarget.localScale.z) : 1F;
            transform.localScale = Vector3.Scale(currentScale, new Vector3(signX, signY, signZ));
        }
    }
}