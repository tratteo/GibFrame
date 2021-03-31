using System;
using UnityEngine.UI;

namespace GibFrame.Patterns
{
    public class HealthSystem
    {
        private float currentHealth;
        private Image healthBar;

        public float MaxHealth { get; private set; }

        public event Action<HealthSystem> OnDamage;

        public event Action<HealthSystem> OnDeath;

        public event Action<HealthSystem> OnHeal;

        public HealthSystem(float maxHealth, Image healthBar = null)
        {
            MaxHealth = maxHealth;
            currentHealth = 0F;
            this.healthBar = healthBar;
        }

        public void Damage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth < 0F) currentHealth = 0F;
            OnDamage?.Invoke(this);
            if (currentHealth == 0F) OnDeath?.Invoke(this);

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
            OnHeal?.Invoke(this);

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
        }
    }
}
