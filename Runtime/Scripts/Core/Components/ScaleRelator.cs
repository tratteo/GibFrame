using UnityEngine;

namespace GibFrame
{
    [ExecuteAlways]
    public class ScaleRelator : MonoBehaviour
    {
        public bool disjoinX;
        public bool disjoinY;
        public bool disjoinZ;

        private void LateUpdate()
        {
            var currentScale = transform.localScale.AsPositive();
            var signX = disjoinX ? Mathf.Sign(transform.root.localScale.x) : 1F;
            var signY = disjoinY ? Mathf.Sign(transform.root.localScale.y) : 1F;
            var signZ = disjoinZ ? Mathf.Sign(transform.root.localScale.z) : 1F;
            transform.localScale = Vector3.Scale(currentScale, new Vector3(signX, signY, signZ));
        }
    }
}
