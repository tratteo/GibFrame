using UnityEngine;

namespace GibFrame
{
    /// <summary>
    ///   Class used to serialize in the inspector a single layer
    /// </summary>
    [System.Serializable]
    public class SerializableLayer
    {
        [SerializeField]
        private int layerIndex = 0;

        public int LayerIndex => layerIndex;

        public int Mask => 1 << layerIndex;

        public void Set(int layerIndex)
        {
            if (layerIndex is > 0 and < 32)
            {
                this.layerIndex = layerIndex;
            }
        }
    }
}