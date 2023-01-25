using GibFrame;
using UnityEngine;
using UnityEngine.UI;

namespace RogueDuel.UI
{
    public class ValueSystemUI : MonoBehaviour
    {
        [Header("Value")]
        [SerializeField] private Image mainBar;
        [SerializeField] private Image mainFill;
        [Header("Delayed effect")]
        [SerializeField] private Image secondBar;
        [SerializeField] private Image secondFill;
        [SerializeField] private float catchUpMultiplier = 0.02F;
        [Header("References")]
        [SerializeField] private Image border;
        [SerializeField] private Text amountText;

        [SerializeField] private SerializableInterface<IValueSystem> preBinded;
        private IValueSystem bindedSystem;

        private float delayedValue = 0;

        public bool IsBinded { get; private set; } = false;

        public void Bind(IValueSystem system)
        {
            Unbind();
            bindedSystem = system;
            bindedSystem.Changed += OnChanged;
            delayedValue = system.Value;
            UpdateUI();
            IsBinded = true;
        }

        public void SetColor(Color32 color)
        {
            if (mainFill) mainFill.color = color;
            if (secondFill)
            {
                secondFill.color = new Color((color.r + 150) / 255F, (color.g + 150) / 255F, (color.b + 150) / 255F, color.a);
            }
            border.color = color;
        }

        public void Unbind()
        {
            if (bindedSystem is not null)
            {
                bindedSystem.Changed -= OnChanged;
            }
            bindedSystem = null;
            IsBinded = false;
        }

        public void Update()
        {
            if (IsBinded && secondBar)
            {
                delayedValue = Mathf.Lerp(delayedValue, bindedSystem.Value, catchUpMultiplier);
                secondBar.fillAmount = delayedValue / bindedSystem.MaxValue;
            }
        }

        private void Start()
        {
            if (preBinded.Value is not null)
            {
                Bind(preBinded.Value);
            }
        }

        private void UpdateUI()
        {
            if (bindedSystem is null) return;
            if (mainBar != null)
            {
                mainBar.fillAmount = bindedSystem.ValuePercentage;
            }
            if (amountText != null)
            {
                amountText.text = bindedSystem.Value.ToString();
            }
        }

        private void OnChanged(float oldVal, float newVal) => UpdateUI();

        private void OnDestroy() => Unbind();
    }
}