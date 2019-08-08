using Dinopark.Life;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

// Require

namespace Assets.Gamelogic.Life
{
    public class HealthVisualizer : MonoBehaviour
    {
        [Require] private HealthReader health;

        public float CurrentHealth { get { return health.Data.CurrentHealth; } }
        public float MaxHealth { get { return health.Data.MaxHealth; } }

        [SerializeField] private string _healthStr;

        void OnEnable()
        {
            health.OnUpdate += OnHealthChanged;
            _healthStr = string.Format(health.Data.CurrentHealth+"/"+health.Data.MaxHealth);
        }

        void OnDisable()
        {
            health.OnUpdate -= OnHealthChanged;
        }

        void OnHealthChanged(Health.Update update)
        {
            float currentHealth = CurrentHealth;
            if (update.CurrentHealth.HasValue)
                currentHealth = update.CurrentHealth.Value;
            float maxHealth = MaxHealth;
            if (update.MaxHealth.HasValue)
                maxHealth = update.MaxHealth.Value;
            _healthStr = string.Format(currentHealth+"/"+maxHealth);
        }
    }
}
