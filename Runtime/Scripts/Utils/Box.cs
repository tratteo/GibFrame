using UnityEngine;

namespace GibFrame.Utils
{
    public struct Box
    {
        public Vector3 FrontTopLeft { get; private set; }

        public Vector3 FrontTopRight { get; private set; }

        public Vector3 FrontBottomLeft { get; private set; }

        public Vector3 FrontBottomRight { get; private set; }

        public Vector3 BackTopLeft { get; private set; }

        public Vector3 BackTopRight { get; private set; }

        public Vector3 BackBottomLeft { get; private set; }

        public Vector3 BackBottomRight { get; private set; }

        public Vector3 Origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
        {
            Rotate(orientation);
        }

        public Box(Vector3 origin, Vector3 halfExtents)
        {
            BackBottomLeft = new Vector3(origin.x - halfExtents.x, origin.y - halfExtents.y, origin.z - halfExtents.z);
            FrontBottomLeft = new Vector3(origin.x - halfExtents.x, origin.y - halfExtents.y, origin.z + halfExtents.z);
            FrontBottomRight = new Vector3(origin.x + halfExtents.x, origin.y - halfExtents.y, origin.z + halfExtents.z);
            BackBottomRight = new Vector3(origin.x + halfExtents.x, origin.y - halfExtents.y, origin.z - halfExtents.z);
            BackTopLeft = BackBottomLeft + Vector3.up * 2F * halfExtents.y;
            FrontTopLeft = FrontBottomLeft + Vector3.up * 2F * halfExtents.y;
            FrontTopRight = FrontBottomRight + Vector3.up * 2F * halfExtents.y;
            BackTopRight = BackBottomRight + Vector3.up * 2F * halfExtents.y;
            this.Origin = origin;
        }

        public void Rotate(Quaternion orientation)
        {
            BackBottomLeft = RotatePointAroundPivot(BackBottomLeft, Vector3.zero, orientation);
            FrontBottomLeft = RotatePointAroundPivot(FrontBottomLeft, Vector3.zero, orientation);
            FrontBottomRight = RotatePointAroundPivot(FrontBottomRight, Vector3.zero, orientation);
            BackBottomRight = RotatePointAroundPivot(BackBottomRight, Vector3.zero, orientation);

            BackTopLeft = RotatePointAroundPivot(BackTopLeft, Vector3.zero, orientation);
            FrontTopLeft = RotatePointAroundPivot(FrontTopLeft, Vector3.zero, orientation);
            FrontTopRight = RotatePointAroundPivot(FrontTopRight, Vector3.zero, orientation);
            BackTopRight = RotatePointAroundPivot(BackTopRight, Vector3.zero, orientation);
        }

        private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            Vector3 direction = point - pivot;
            return pivot + rotation * direction;
        }
    }
}
