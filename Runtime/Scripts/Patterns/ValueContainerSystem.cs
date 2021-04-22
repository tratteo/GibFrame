using System;
using UnityEngine.UI;

namespace GibFrame.Patterns
{
    public class ValueContainerSystem
    {
        private readonly Image valueBar;
        private readonly Text valueText;
        private readonly string valueTextFormat;

        public float CurrentValue { get; private set; }

        public float MaxValue { get; private set; }

        public event Action<float> OnDecrease;

        public event Action OnExhaust;

        public event Action<float> OnIncrease;

        public ValueContainerSystem(float maxValue, Image valueBar, Text valueText, string valueTextFormat)
        {
            MaxValue = maxValue;
            CurrentValue = maxValue;
            this.valueBar = valueBar;
            this.valueText = valueText;
            this.valueTextFormat = valueTextFormat;
            AdjustVisuals();
        }

        public ValueContainerSystem(float maxHealth) : this(maxHealth, null, null, "")
        {
        }

        public ValueContainerSystem(float maxHealth, Image healthBar) : this(maxHealth, healthBar, null, "")
        {
        }

        public ValueContainerSystem(float maxHealth, Image healthBar, Text healthText) : this(maxHealth, healthBar, healthText, "")
        {
        }

        public ValueContainerSystem(float maxHealth, Text healthText, string healthTextFormat) : this(maxHealth, null, healthText, healthTextFormat)
        {
        }

        public void Decrease(float amount)
        {
            CurrentValue -= amount;
            if (CurrentValue < 0F) CurrentValue = 0F;
            OnDecrease?.Invoke(amount);
            if (CurrentValue == 0F) OnExhaust?.Invoke();

            AdjustVisuals();
        }

        public void Exhaust()
        {
            Decrease(MaxValue);
        }

        public float GetPercentage() => CurrentValue / MaxValue;

        public void Increase(float amount)
        {
            CurrentValue += amount;
            if (CurrentValue > MaxValue) CurrentValue = MaxValue;
            OnIncrease?.Invoke(amount);
            AdjustVisuals();
        }

        public void Refull()
        {
            Increase(MaxValue);
        }

        public void SetMaxValue(float maxHealth)
        {
            MaxValue = maxHealth;
        }

        private void AdjustVisuals()
        {
            if (valueBar != null)
            {
                valueBar.fillAmount = GetPercentage();
            }
            if (valueText != null)
            {
                if (valueTextFormat != "")
                {
                    valueText.text = string.Format(valueTextFormat, CurrentValue);
                }
                else
                {
                    valueText.text = CurrentValue.ToString();
                }
            }
        }
    }
}
