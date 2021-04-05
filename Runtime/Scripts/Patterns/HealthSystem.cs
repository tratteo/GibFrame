using System;
using UnityEngine.UI;

namespace GibFrame.Patterns
{
    public class HealthSystem
    {
        private float currentHealth;
        private Image healthBar;
        private Text healthText;

        public float MaxHealth { get; private set; }

        public event Action<float> OnDamage;

        public event Action OnDeath;

        public event Action<float> OnHeal;

        public HealthSystem(float maxHealth, Image healthBar = null, Text healthText = null)
        {
            MaxHealth = maxHealth;
            currentHealth = 0F;
            this.healthBar = healthBar;
            this.healthText = healthText;
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
            if (currentHealth > amount) currentHealth = amount;
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
                healthText.text = currentHealth.ToString();
            }
        }
    }
}
