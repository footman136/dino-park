using Dinopark.Life;
using UnityEngine;

namespace Assets.Gamelogic.ComponentExtensions
{
    static class HealthExtensions
    {
        public static void SetCanBeChanged(this HealthWriter health, bool canBeChanged)
        {
            var update = new Health.Update()
            {
                CanBeChanged = canBeChanged
            };
            health.SendUpdate(update);
        }

        public static void SetCurrentHealth(this HealthWriter health, int newHealth)
        {
            if (health.Data.CanBeChanged)
            {
                var update = new Health.Update()
                {
                    CurrentHealth = Mathf.Max(newHealth, 0)
                };
                health.SendUpdate(update);
            }
        }

        public static void AddCurrentHealthDelta(this HealthWriter health, int delta)
        {
            if (health.Data.CanBeChanged)
            {
                if (health.TryingToDecreaseHealthBelowZero(delta))
                {
                    return;
                }

                var update = new Health.Update()
                {
                    CurrentHealth = Mathf.Max(health.Data.CurrentHealth + delta, 0)
                };
                health.SendUpdate(update);
            }
        }

        private static bool TryingToDecreaseHealthBelowZero(this HealthReader health, int delta)
        {
            return health.Data.ComponentId == 0 && delta < 0;
        }
    }
}
