using System;

namespace GibFrame
{
    [Serializable]
    public class ValueSystem
    {
        private float currentValue;
        private float maxValue;

        public float Value => currentValue;

        public float MaxValue => maxValue;

        public float ValuePercentage => Value / maxValue;

        public ValueSystem(float maxValue)
        {
            this.maxValue = maxValue;
            currentValue = maxValue;
        }

        public event Action<float> OnDecrease;

        public event Action OnExhaust;

        public event Action<float> OnIncrease;

        public event Action<float, float> Changed;

        public void Decrease(float amount)
        {
            float old = currentValue;
            currentValue -= amount;
            if (Value < 0F) currentValue = 0F;
            OnDecrease?.Invoke(amount);
            Changed?.Invoke(old, currentValue);
            if (Value == 0F) OnExhaust?.Invoke();
        }

        public void Exhaust() => Decrease(maxValue);

        public void Increase(float amount)
        {
            float old = currentValue;
            currentValue += amount;
            if (Value > maxValue) currentValue = maxValue;
            Changed?.Invoke(old, currentValue);
            OnIncrease?.Invoke(amount);
        }

        public void Refull() => Increase(maxValue);

        public void SetMaxValue(float maxHealth)
        {
            maxValue = maxHealth;
        }
    }
}