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
        //[Require] private HarvestableWriter harvestable;
        [Require] private HealthWriter health;
        [Require] private HarvestableCommandReceiver cmdReceiver;

        private void OnEnable()
        {
            cmdReceiver.OnHarvestRequestReceived += (OnHarvest);
        }

        private void OnDisable()
        {
            cmdReceiver.OnHarvestRequestReceived -= (OnHarvest);
        }

        private void OnHarvest(Harvestable.Harvest.ReceivedRequest request)
        {
            var payload = request.Payload;
            var nowHealth = health.Data.CurrentHealth;
            float resTaken = 0;
            if (nowHealth > payload.ResourcesNeed)
            {
                resTaken = payload.ResourcesNeed;
                nowHealth -= payload.ResourcesNeed;
            }
            else
            {
                resTaken = nowHealth;
                nowHealth = 0;
            }

            var update = new HarvestResponse()
            {
                ResourcesTaken = resTaken
            };
            cmdReceiver.SendHarvestResponse(request.RequestId, update);
            //Debug.Log("OnHarvest ResourceNeed<"+payload.ResourcesNeed+"> Harvester<"+payload.Harvester.Id+">");

            var update2 = new Health.Update
            {
                CurrentHealth = nowHealth
            };
            health.SendUpdate(update2);
        }
    }
}
