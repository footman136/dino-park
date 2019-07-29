using Dinopark.Fire;
using Assets.Gamelogic.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
// WorkerUtils
// flammable

// Require, WorkerType

namespace Assets.Gamelogic.Fire
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class FlammableDataVisualizer : MonoBehaviour
    {
        [Require] private FlammableReader flammable;
        public bool canBeIgnited { get; private set; }

        void OnEnable()
        {
            flammable.OnUpdate += (FlammableOnComponentUpdated);
            canBeIgnited = flammable.Data.CanBeIgnited;
        }

        void OnDisable()
        {
            flammable.OnUpdate -= (FlammableOnComponentUpdated);
            canBeIgnited = false;
        }

        private void FlammableOnComponentUpdated(Flammable.Update update)
        {
            canBeIgnited = flammable.Data.CanBeIgnited;
        }

        public void SetLocalCanBeIgnited(bool ignitable)
        {
            canBeIgnited = ignitable;
        }
    }
}
