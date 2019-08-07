using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Improbable.Gdk.Core;

public class DinoWanderState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    public DinoWanderState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;
    }
    public override void Enter()
    {
        parentBehaviour.navMeshAgent.SetDestination(Owner.Data.TargetPosition.ToUnityVector());
    }

    public override void Tick()
    {
        int arrived = 0;
        if (parentBehaviour.navMeshAgent.remainingDistance < parentBehaviour.navMeshAgent.stoppingDistance)
        {
            arrived = 1;
        }

        float deltaTime = Time.time - Owner._startTime;
        if (deltaTime > parentBehaviour.ScriptableAnimalStats.stamina)
        {
            arrived = 2;
            if (parentBehaviour.logChanges)
            {
                Debug.Log("Time's up.");
            }
        }

        if (arrived>0)
        {
            Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
