using Assets.Gamelogic.Utils;
using Assets.Gamelogic.Core;
using Dinopark.Fire;
using DinoPark;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
// SimulationSettings
// Nothing
// flammable
//using Improbable.Entity.Component;
//using Improbable.Unity.Visualizer;
// Require, WorkerType

// WorkerUtils

namespace Assets.Gamelogic.Fire
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class FlammableBehaviour : MonoBehaviour
    {
        [Require] private FlammableWriter flammable;
        [Require] private FlammableCommandReceiver receiver;
        [Require] private FlammableCommandSender sender;
        

        public bool IsOnFire { get { return flammable != null && flammable.Data.IsOnFire; } }
        private Collider[] nearbyColliders = new Collider[8];
        private Coroutine spreadFireCoroutine;

        private IFlammable[] flammableInterfaces;

        private void Awake()
        {
            flammableInterfaces = gameObject.GetComponents<IFlammable>();
        }

        private void OnEnable()
        {
            receiver.OnIgniteRequestReceived += OnIgnite; // 点燃
            receiver.OnExtinguishRequestReceived += OnExtinguish;
            receiver.OnSetCanBeIgnitedRequestReceived += OnSetCanBeIgnited;
                
            if (flammable.Data.IsOnFire)
            {
                StartSpreadingFire();
            }
        }

        private void OnDisable()
        {
            receiver.OnIgniteRequestReceived -= OnIgnite;
            receiver.OnExtinguishRequestReceived -= OnExtinguish;
            receiver.OnSetCanBeIgnitedRequestReceived -= OnSetCanBeIgnited;

            StopSpreadingFire();
        }

        private void OnIgnite(Flammable.Ignite.ReceivedRequest request)
        {
            Ignite();
        }

        private void OnExtinguish(Flammable.Extinguish.ReceivedRequest request)
        {
            Extinguish(false);
        }

        private void OnSetCanBeIgnited(Flammable.SetCanBeIgnited.ReceivedRequest request)
        {
            SetCanBeIgnited(flammable.Data.CanBeIgnited);
        }

        private void Ignite()
        {
            if (!flammable.Data.IsOnFire && flammable.Data.CanBeIgnited)
            {
                SendIgniteUpdate();
                StartSpreadingFire();
                for (var i = 0; i < flammableInterfaces.Length; i++)
                {
                    flammableInterfaces[i].OnIgnite();
                }
            }
        }

        private void Extinguish(bool canBeIgnited)
        {
            if (flammable.Data.IsOnFire)
            {
                SendExtinguishUpdate(canBeIgnited);
                StopSpreadingFire();
                for (var i = 0; i < flammableInterfaces.Length; i++)
                {
                    flammableInterfaces[i].OnExtinguish();
                }
            }
        }

        private void SetCanBeIgnited(bool canBeIgnited)
        {
            if (flammable.Data.CanBeIgnited != canBeIgnited)
            {
                var update = new Flammable.Update()
                {
                    CanBeIgnited = canBeIgnited
                };
                flammable.SendUpdate(update);
            }
        }

        private void SelfIgnite()
        {
            if (flammable == null)
            {
                Flammable.Ignite.Request request = new Flammable.Ignite.Request();
                sender.SendIgniteCommand(request);
                return;
            }
            Ignite();
        }

        public void SelfExtinguish()
        {
            if (flammable == null)
            {
                Flammable.Extinguish.Request request = new Flammable.Extinguish.Request();
                sender.SendExtinguishCommand(request);
            }

            Extinguish(false);
        }

        public void SelfSetCanBeIgnited(bool canBeIgnited)
        {
            if (flammable == null)
            {
                Flammable.SetCanBeIgnited.Request request = new Flammable.SetCanBeIgnited.Request();
                sender.SendSetCanBeIgnitedCommand(request);
                return;
            }
            SetCanBeIgnited(canBeIgnited);
        }

        private void StartSpreadingFire()
        {
            spreadFireCoroutine = StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.FireSpreadInterval, SpreadFire));
        }

        private void StopSpreadingFire()
        {
            if (spreadFireCoroutine != null)
            {
                StopCoroutine(spreadFireCoroutine);
            }
        }

        private void SpreadFire()
        {
            if (flammable == null)
            {
                return;
            }

            var count = Physics.OverlapSphereNonAlloc(transform.position, SimulationSettings.FireSpreadRadius, nearbyColliders);
            for (var i = 0; i < count; i++)
            {
                var otherFlammable = nearbyColliders[i].transform.GetComponentInParent<FlammableDataVisualizer>();
                if (otherFlammable != null && otherFlammable.canBeIgnited)
                {
                    // Cache local ignitable value, to avoid duplicated ignitions within 1 frame on an UnityWorker
                    otherFlammable.SetLocalCanBeIgnited(false);
                    otherFlammable.GetComponent<FlammableBehaviour>().SelfIgnite();
                }
            }
        }

        private void SendIgniteUpdate()
        {
            var update = new Flammable.Update()
            {
                IsOnFire = true,
                CanBeIgnited = false
            };
            flammable.SendUpdate(update);
        }

        private void SendExtinguishUpdate(bool canBeIgnited)
        {
            var update = new Flammable.Update()
            {
                IsOnFire = false,
                CanBeIgnited = canBeIgnited
            };
            flammable.SendUpdate(update);
        }
    }
}
