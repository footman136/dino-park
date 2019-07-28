using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Dinopark.Fire;
using Dinopark.Life;
using DinoPark;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
// MonoBehaviour
// WorkerUtils
// SimulationSettings
// Quaternion, MathUtils, TimeUtils
// flammable
// Health

// Require, WorkerType


namespace Assets.Gamelogic.Fire
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    class FireHealthInteractionBehaviour : MonoBehaviour
    {
        [Require] private HealthWriter health;
        [Require] private FlammableReader flammable;

        // note: This parameter is overwritable if you want to control the speed at which an entity takes damage, e.g. for trees
        public float FireDamageInterval = SimulationSettings.DefaultFireDamageInterval;
        private Coroutine takeDamageFromFireCoroutine;

        private void OnEnable()
        {
            if (flammable.Data.IsOnFire)
            {
                StartDamageRoutine();
            }

            flammable.OnUpdate += (FlammableOnComponentUpdated);
        }

        private void OnDisable()
        {
            CancelDamageRoutine();

            flammable.OnUpdate -= (FlammableOnComponentUpdated);
        }

        private void FlammableOnComponentUpdated(Flammable.Update update)
        {
            if (update.IsOnFire.HasValue)
            {
                if (flammable.Data.IsOnFire)
                {
                    StartDamageRoutine();
                }
                else
                {
                    CancelDamageRoutine();
                }
            }
        }

        private void StartDamageRoutine()
        {
            if (takeDamageFromFireCoroutine != null)
            {
                return;
            }
            takeDamageFromFireCoroutine = StartCoroutine(TimerUtils.CallRepeatedly(SimulationSettings.SimulationTickInterval * FireDamageInterval, TakeDamageFromFire));
        }

        private void CancelDamageRoutine()
        {
            if (takeDamageFromFireCoroutine != null)
            {
                StopCoroutine(takeDamageFromFireCoroutine);
                takeDamageFromFireCoroutine = null;
            }
        }

        private void TakeDamageFromFire()
        {
            var update = new Health.Update()
            {
                CurrentHealth = health.Data.CurrentHealth - SimulationSettings.FireDamagePerTick
            };
            health.SendUpdate(update);
        }
    }
}
