using UnityEngine;
using UnityEngine.Events;

namespace GibFrame.Selectors
{
    public class Selectable : MonoBehaviour, ISelectable
    {
        [SerializeField] private UnityEvent onSelect;
        [SerializeField] private UnityEvent onDeselect;

        public void OnDeselect()
        {
            onDeselect?.Invoke();
        }

        public void OnSelect()
        {
            onSelect?.Invoke();
        }
    }
}