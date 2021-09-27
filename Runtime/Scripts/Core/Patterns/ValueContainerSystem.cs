// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : ValueContainerSystem.cs
//
// All Rights Reserved

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

        public string TextFormat { get; set; }

        public float MaxValue => maxValue;

        public event Action<float> OnDecrease;

        public event Action OnExhaust;

        public event Action<float> OnIncrease;

        public ValueContainerSystem(float maxValue, Image bar, Text text, string textFormat)
        {
            this.maxValue = maxValue;
            currentValue = maxValue;
            this.valueBar = bar;
            this.valueText = text;
            this.TextFormat = textFormat;
            AdjustVisuals();
        }

        public ValueContainerSystem(float maxValue) : this(maxValue, null, null, "")
        {
        }

        public ValueContainerSystem(float maxValue, Image bar) : this(maxValue, bar, null, "")
        {
        }

        public ValueContainerSystem(float maxValue, Image bar, Text text) : this(maxValue, bar, text, "")
        {
        }

        public ValueContainerSystem(float maxValue, Text text, string textFormat) : this(maxValue, null, text, textFormat)
        {
        }

        public ValueContainerSystem(float maxValue, string textFormat) : this(maxValue, null, null, textFormat)
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
                if (TextFormat != string.Empty)
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
