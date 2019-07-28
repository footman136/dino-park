using Assets.Gamelogic.Core;
using Assets.Gamelogic.Fire;
using Dinopark.Fire;
using Dinopark.Life;
using Dinopark.Plants;
using DinoPark;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
// WorkerUtils
// SimulationSettings
// flammable

// Require, WorkerType

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class TreeBehaviour : MonoBehaviour
    {
        [Require] private TreeStateWriter tree;
        [Require] private FlammableWriter flammable;
        [Require] private HealthWriter health;

        [SerializeField] private FlammableBehaviour flammableInterface;

        private TreeStateMachine stateMachine;

        private void Awake()
        {
            flammableInterface = gameObject.GetComponentIfUnassigned(flammableInterface);
        }

        private void OnEnable()
        {
            stateMachine = new TreeStateMachine(this, 
                tree,
                health,
                flammableInterface,
                flammable);

            stateMachine.OnEnable(tree.Data.CurrentState);
        }

        private void OnDisable()
        {
            stateMachine.OnDisable();
        }

    }
}
