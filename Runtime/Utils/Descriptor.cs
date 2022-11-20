using GibFrame.Meta;
using UnityEngine;

namespace GibFrame
{
    /// <summary>
    ///   <see cref="ScriptableObject"/> defining something that can be described
    /// </summary>
    [CreateAssetMenu(menuName = "GibFrame/Descriptor", fileName = "custom_desc")]
    public class Descriptor : ScriptableObject
    {
        [SerializeField, Guid(Resettable = true)] private string guid;
        [SerializeField] private new string name;
        [SerializeField] private Sprite icon;
        [SerializeField, TextArea] private string description;

        public string Name => name;

        public string Guid => guid;

        public Sprite Icon => icon;

        public string Description => description;
    }
}