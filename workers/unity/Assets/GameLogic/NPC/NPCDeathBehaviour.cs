using Assets.Gamelogic.Core;
using Dinopark.Life;
using UnityEngine;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core;

namespace Assets.Gamelogic.NPC
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class NPCDeathBehaviour : MonoBehaviour
    {
        [Require] private HealthReader health;
        private WorldCommandSender worldSender;
        

        private bool npcDeathActive;

        private void OnEnable()
        {
            npcDeathActive = SimulationSettings.NPCDeathActive;
            health.OnUpdate += (OnHealthUpdated);
        }

        private void OnDisable()
        {
            health.OnUpdate -= (OnHealthUpdated);
        }

        private void OnHealthUpdated(Health.Update update)
        {
            if (update.CurrentHealth.HasValue)
            {
                DieUponHealthDepletion(update);
            }
        }

        private void DieUponHealthDepletion(Health.Update update)
        {
            if (npcDeathActive && update.CurrentHealth.Value <= 0)
            {
                var linkedComponent = gameObject.GetComponent<LinkedEntityComponent>(); // 终于找到一个gameObject和EntityId挂钩的地方
                if (linkedComponent != null)
                {
                    var request = new WorldCommands.DeleteEntity.Request
                    {
                        EntityId = linkedComponent.EntityId,
                        TimeoutMillis = 1
                    };
                    worldSender.SendDeleteEntityCommand(request);
                }
            }
        }
    }
}