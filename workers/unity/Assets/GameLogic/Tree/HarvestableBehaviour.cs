using Assets.Gamelogic.Core;
using Dinopark.Life;
using Dinopark.Plants;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
// SimulationSettings
// Health

// Require, WorkerType

namespace Assets.Gamelogic.Tree
{
    public class HarvestableBehaviour : MonoBehaviour
    {
        [Require] private HarvestableWriter harvestable;
        [Require] private HealthWriter health;

        private void OnEnable()
        {
            harvestable.OnUpdate += (OnHarvest);
        }

        private void OnDisable()
        {
            harvestable.OnUpdate -= (OnHarvest);
        }

        private void OnHarvest(Harvestable.Update update)
        {
            var resourcesToGive = Mathf.Min(SimulationSettings.HarvestReturnQuantity, health.Data.CurrentHealth);
            var update2 = new Health.Update
            {
                CurrentHealth = health.Data.CurrentHealth - resourcesToGive
            };
            health.SendUpdate(update2);
        }
    }
}
