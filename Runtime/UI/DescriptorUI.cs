using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GibFrame.UI
{
    public class DescriptorUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameTxt;
        [SerializeField] private TMP_Text descriptionTxt;
        [SerializeField] private Image iconImg;

        public TMP_Text NameObj => nameTxt;

        public TMP_Text DescriptionObj => descriptionTxt;

        public Image Icon => iconImg;

        public void Clear()
        {
            if (nameTxt is not null) nameTxt.text = string.Empty;
            if (descriptionTxt is not null) descriptionTxt.text = string.Empty;
            if (iconImg is not null)
            {
                iconImg.sprite = null;
                iconImg.gameObject.SetActive(false);
            }
        }

        public void Display(IDescriptable descriptable) => Display(descriptable.GetDescriptor());

        public void Display(Descriptor descriptor)
        {
            if (descriptor == null) return;
            if (nameTxt is not null) nameTxt.text = descriptor.Name;
            if (descriptionTxt is not null) descriptionTxt.text = descriptor.Description;
            if (iconImg is not null)
            {
                iconImg.sprite = descriptor.Icon;
                iconImg.gameObject.SetActive(descriptor.Icon);
            }
        }

        private void Awake()
        {
            Clear();
        }
    }
}