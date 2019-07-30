using Dinopark.Life;
using UnityEngine;
using Improbable.Gdk.Subscriptions;

namespace Assets.Gamelogic.Core
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class CharacterDeathVisualizer : MonoBehaviour
    {
        [Require] private HealthReader health;

        [SerializeField] private CharacterModelVisualizer characterModelVisualizer;

        private void Awake()
        {
            characterModelVisualizer = gameObject.GetComponentIfUnassigned(characterModelVisualizer);
        }

        private void OnEnable()
        {
            characterModelVisualizer.SetModelVisibility(true);
            health.OnUpdate += (HealthUpdated);
        }

        private void OnDisable()
        {
            health.OnUpdate -= (HealthUpdated);
        }

        private void HealthUpdated(Health.Update update)
        {
            if (update.CurrentHealth.HasValue && update.CurrentHealth.Value <= 0)
            {
                PlayDeathAnimation();
            }
        }

        private void PlayDeathAnimation()
        {
            DeathAnimVisualizerPool.ShowEffect(transform.position);
            characterModelVisualizer.SetModelVisibility(false);
        }
    }
}
