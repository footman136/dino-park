using Assets.Gamelogic.UI;
using Dinopark.Fire;
using DinoPark;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
// WorkerUtils
// flammable

// Require, WorkerType

namespace Assets.Gamelogic.Fire
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class FlammableVisualizer : MonoBehaviour
    {
        [Require] private FlammableReader flammable;

        private GameObject fireEffectInstance;
        private ParticleSystem fireEffectparticleSystem;

        private void CreateFireEffectInstance()
        {
            switch (flammable.Data.EffectType)
            {
                case FireEffectType.BIG:
                    fireEffectInstance = (GameObject)Instantiate(ResourceRegistry.FirePrefab, transform);
                    break;
                case FireEffectType.SMALL:
                    fireEffectInstance = (GameObject)Instantiate(ResourceRegistry.SmallFirePrefab, transform);
                    break;
            }
            fireEffectInstance.transform.localPosition = Vector3.zero;
            fireEffectparticleSystem = fireEffectInstance.GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            if (fireEffectInstance == null)
            {
                CreateFireEffectInstance();
            }
            flammable.OnUpdate += (OnComponentUpdated);
            UpdateParticleSystem(flammable.Data.IsOnFire);
        }

        private void OnDisable()
        {
            flammable.OnUpdate -= (OnComponentUpdated);
        }

        private void OnComponentUpdated(Flammable.Update update)
        {
            if(update.IsOnFire.HasValue)
            {
                UpdateParticleSystem(update.IsOnFire.Value);
            }
        }

        private void UpdateParticleSystem(bool enabled)
        {
            if(enabled)
            {
                fireEffectparticleSystem.Play();
            }
            else
            {
                fireEffectparticleSystem.Stop();
            }
        }
    }
}
