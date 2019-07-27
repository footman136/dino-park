using UnityEngine;
using DinoPark; // WorkerUtils
using GameLogic.Core; // SimulationSettings
using GameLogic.Fire;
using Dinopark.Fire; // flammable
using Dinopark.Life;
using Dinopark.Plants;
using Improbable.Gdk.Subscriptions; // Require, WorkerType

namespace GameLogic.Tree
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
