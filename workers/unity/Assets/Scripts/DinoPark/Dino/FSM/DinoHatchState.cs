using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable.Gdk.Core;

public class DinoHatchState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    private Coroutine hatchingCoroutine;

    public DinoHatchState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;        
    }
    public override void Enter()
    {
        parentBehaviour.navMeshAgent.SetDestination(parentBehaviour.transform.position);
        hatchingCoroutine = parentBehaviour.StartCoroutine(TimerUtils.WaitAndPerform(parentBehaviour.ScriptableAnimalStats.layEggTime, LayEgg));
        
    }

    public override void Tick()
    {
    }

    public override void Exit(bool disabled)
    {
        parentBehaviour.StopCoroutine(hatchingCoroutine);
        hatchingCoroutine = null;
    }

    private void LayEgg()
    {
        parentBehaviour.LayEgg();
        Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
    }
}
