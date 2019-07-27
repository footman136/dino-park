using UnityEngine;
using GameLogic.Core; // SimulationSettings
using Dinopark.Life; // Health
using Dinopark.Plants;
using Improbable.Gdk.Subscriptions; // Require, WorkerType

namespace GameLogic.Tree
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
