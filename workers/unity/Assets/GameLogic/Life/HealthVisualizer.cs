using Dinopark.Life;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

// Require

namespace Assets.Gamelogic.Life
{
    public class HealthVisualizer : MonoBehaviour
    {
        [Require] private HealthReader health;

        public int CurrentHealth { get { return health.Data.CurrentHealth; } }
        public int MaxHealth { get { return health.Data.MaxHealth; } }
    }
}
