using Dinopark.Life;
using UnityEngine;
using Improbable.Gdk.Subscriptions; // Require

namespace Gamelogic.Life
{
    public class HealthVisualizer : MonoBehaviour
    {
        [Require] private HealthReader health;

        public int CurrentHealth { get { return health.Data.CurrentHealth; } }
        public int MaxHealth { get { return health.Data.MaxHealth; } }
    }
}
