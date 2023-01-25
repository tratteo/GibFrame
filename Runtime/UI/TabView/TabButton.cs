using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GibFrame.UI
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public enum TabState
        { IDLE, SELECTED, HOVER }

        [SerializeField] private Color selected;
        [SerializeField] private Color idle;
        [SerializeField] private Color hover;

        private Image background;

        public TabGroup Group { get; set; } = null;

        public Sprite Icon { get; set; } = null;

        public void OnPointerClick(PointerEventData eventData)
        {
            Group.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Group.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Group.OnTabExit(this);
        }

        public void SetState(TabState state)
        {
            switch (state)
            {
                case TabState.IDLE:
                    background.color = idle;
                    break;

                case TabState.SELECTED:
                    background.color = selected;
                    break;

                case TabState.HOVER:
                    background.color = hover;
                    break;
            }
        }

        private void Start()
        {
            if (Icon != null)
            {
                background.sprite = Icon;
            }
        }

        private void Awake()
        {
            background = GetComponent<Image>();
            Group = GetComponentInParent<TabGroup>();
            Group?.Subscribe(this);
        }
    }
}