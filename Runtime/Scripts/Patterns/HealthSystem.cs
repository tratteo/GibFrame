using System;
using UnityEngine.UI;

namespace GibFrame.Patterns
{
    public class HealthSystem
    {
        private readonly Image healthBar;
        private readonly Text healthText;
        private readonly string healthTextFormat;

        public float CurrentHealth { get; private set; }

        public float MaxHealth { get; private set; }

        public event Action<float> OnDamage;

        public event Action OnDeath;

        public event Action<float> OnHeal;

        public HealthSystem(float maxHealth, Image healthBar, Text healthText, string healthTextFormat)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            this.healthBar = healthBar;
            this.healthText = healthText;
            this.healthTextFormat = healthTextFormat;
            AdjustHealthBar();
        }

        public HealthSystem(float maxHealth) : this(maxHealth, null, null, "")
        {
        }

        public HealthSystem(float maxHealth, Image healthBar) : this(maxHealth, healthBar, null, "")
        {
        }

        public HealthSystem(float maxHealth, Image healthBar, Text healthText) : this(maxHealth, healthBar, healthText, "")
        {
        }

        public HealthSystem(float maxHealth, Text healthText, string healthTextFormat) : this(maxHealth, null, healthText, healthTextFormat)
        {
        }

        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth < 0F) CurrentHealth = 0F;
            OnDamage?.Invoke(amount);
            if (CurrentHealth == 0F) OnDeath?.Invoke();

            AdjustHealthBar();
        }

        public void Exhaust()
        {
            Damage(MaxHealth);
        }

        public float GetPercentage() => CurrentHealth / MaxHealth;

        public void Heal(float amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
            OnHeal?.Invoke(amount);
            AdjustHealthBar();
        }

        public void Refull()
        {
            Heal(MaxHealth);
        }

        public void SetMaxHealth(float maxHealth)
        {
            MaxHealth = maxHealth;
        }

        private void AdjustHealthBar()
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = GetPercentage();
            }
            if (healthText != null)
            {
                if (healthTextFormat != "")
                {
                    healthText.text = string.Format(healthTextFormat, CurrentHealth);
                }
                else
                {
                    healthText.text = CurrentHealth.ToString();
                }
            }
        }
    }
}
