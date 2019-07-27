using UnityEngine;
using DinoPark; // WorkerUtils
using GameLogic.Core; // SimulationSettings, 
using Dinopark.Fire; // flammable
using Improbable.Gdk.Subscriptions; // Require, WorkerType

namespace GameLogic.Fire
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class FlammableAudioTriggers : MonoBehaviour
    {
        [Require] private FlammableReader flammable;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip Ignite;
        [SerializeField] private AudioClip Extinguish;
        [SerializeField] private AudioClip Fire;

        private void Awake()
        {
            audioSource = gameObject.GetComponentIfUnassigned(audioSource);
            audioSource.enabled = true;
        }

        private void OnEnable()
        {
            flammable.OnUpdate += (OnFireChange);
        }

        private void OnDisable()
        {
            flammable.OnUpdate -= (OnFireChange);
        }

        private void OnFireChange(Flammable.Update fireChange)
        {
            if (fireChange.IsOnFire.HasValue)
            {
                if (fireChange.IsOnFire.Value)
                {
                    TriggerIgnitionSound();
                    StartFireAudio();
                }
                else
                {
                    StopFireAudio();
                    TriggerExtinguishSound();
                }
            }
        }

        public void TriggerIgnitionSound()
        {
            audioSource.volume = SimulationSettings.IgnitionVolume;
            audioSource.PlayOneShot(Ignite);
        }

        private void StartFireAudio()
        {
            audioSource.clip = Fire;
            audioSource.volume = SimulationSettings.FireVolume;
            audioSource.loop = true;
            audioSource.Play();
        }

        private void StopFireAudio()
        {
            audioSource.loop = false;
            audioSource.Stop();
        }

        public void TriggerExtinguishSound()
        {
            audioSource.volume = SimulationSettings.ExtinguishVolume;
            audioSource.PlayOneShot(Extinguish);
        }
    }
}
