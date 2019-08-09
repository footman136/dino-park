using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Improbable.Gdk.Core;

public class DinoLookForFoodState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    public DinoLookForFoodState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;      
    }
    public override void Enter()
    {
        parentBehaviour.navMeshAgent.SetDestination(Owner.Data.TargetPosition.ToUnityVector());
        parentBehaviour.navMeshAgent.speed = parentBehaviour.ScriptableAnimalStats.moveSpeed;
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
            if (parentBehaviour.ScriptableAnimalStats.vegetarian)
            {
                Owner.TriggerTransition(DinoAiFSMState.StateEnum.EAT_FOOD, Owner.Data.TargetEntityId,
                    Owner.Data.TargetPosition.ToUnityVector());
            }
            else
            {
                Owner.TriggerTransition(DinoAiFSMState.StateEnum.EAT, Owner.Data.TargetEntityId,
                    Owner.Data.TargetPosition.ToUnityVector());
            }
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
