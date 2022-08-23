using UnityEngine;

namespace GibFrame
{
    /// <summary>
    ///   Class used to serialize in the inspector a single layer
    /// </summary>
    [System.Serializable]
    public class SerializableLayer
    {
        [SerializeField] private int layerIndex = 0;

        public int LayerIndex => layerIndex;

        public int Mask => 1 << layerIndex;

        public static implicit operator LayerMask(SerializableLayer sLayer) => sLayer.Mask;
    }
}