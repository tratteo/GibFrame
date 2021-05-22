//Copyright (c) matteo
//ValueContainerSystem.cs - com.tratteo.gibframe

using System;
using UnityEngine.UI;

namespace GibFrame
{
    [Serializable]
    public class ValueContainerSystem
    {
        private readonly Image valueBar;
        private readonly Text valueText;
        private float currentValue;
        private float maxValue;

        public float CurrentValue => currentValue;

        public string TextFormat { get; private set; }

        public float MaxValue => maxValue;

        public event Action<float> OnDecrease;

        public event Action OnExhaust;

        public event Action<float> OnIncrease;

        public ValueContainerSystem(float maxValue, Image valueBar, Text valueText, string valueTextFormat)
        {
            this.maxValue = maxValue;
            currentValue = maxValue;
            this.valueBar = valueBar;
            this.valueText = valueText;
            this.TextFormat = valueTextFormat;
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

        public ValueContainerSystem(float maxHealth, string healthTextFormat) : this(maxHealth, null, null, healthTextFormat)
        {
        }

        public void Decrease(float amount)
        {
            currentValue -= amount;
            if (CurrentValue < 0F) currentValue = 0F;
            OnDecrease?.Invoke(amount);
            if (CurrentValue == 0F) OnExhaust?.Invoke();

            AdjustVisuals();
        }

        public void Exhaust()
        {
            Decrease(maxValue);
        }

        public float GetPercentage() => CurrentValue / maxValue;

        public void Increase(float amount)
        {
            currentValue += amount;
            if (CurrentValue > maxValue) currentValue = maxValue;
            OnIncrease?.Invoke(amount);
            AdjustVisuals();
        }

        public void Refull()
        {
            Increase(maxValue);
        }

        public void SetMaxValue(float maxHealth)
        {
            maxValue = maxHealth;
        }

        private void AdjustVisuals()
        {
            if (valueBar != null)
            {
                valueBar.fillAmount = GetPercentage();
            }
            if (valueText != null)
            {
                if (TextFormat != "")
                {
                    valueText.text = string.Format(TextFormat, CurrentValue);
                }
                else
                {
                    valueText.text = CurrentValue.ToString();
                }
            }
        }
    }
}