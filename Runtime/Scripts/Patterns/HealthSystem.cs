using System;
using UnityEngine.UI;

namespace GibFrame.Patterns
{
    public class HealthSystem
    {
        private float currentHealth;
        private Image healthBar;
        private Text healthText;
        private string healthTextFormat;

        public float MaxHealth { get; private set; }

        public event Action<float> OnDamage;

        public event Action OnDeath;

        public event Action<float> OnHeal;

        public HealthSystem(float maxHealth, Image healthBar, Text healthText, string healthTextFormat)
        {
            MaxHealth = maxHealth;
            currentHealth = 0F;
            this.healthBar = healthBar;
            this.healthText = healthText;
            this.healthTextFormat = healthTextFormat;
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
            currentHealth -= amount;
            if (currentHealth < 0F) currentHealth = 0F;
            OnDamage?.Invoke(amount);
            if (currentHealth == 0F) OnDeath?.Invoke();

            AdjustHealthBar();
        }

        public void Exhaust()
        {
            Damage(MaxHealth);
        }

        public float GetPercentage() => currentHealth / MaxHealth;

        public void Heal(float amount)
        {
            currentHealth += amount;
            if (currentHealth > MaxHealth) currentHealth = MaxHealth;
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
                    healthText.text = string.Format(healthTextFormat, currentHealth);
                }
                else
                {
                    healthText.text = currentHealth.ToString();
                }
            }
        }
    }
}
