using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;


public class EggGoodState : FsmBaseState<EggStateMachine, EggStateEnum>
{
    private readonly EggBehaviour parentBehaviour;
    private Coroutine hatchingCoroutine;
    
    public EggGoodState(EggStateMachine owner, EggBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;
    }

    public override void Enter()
    {
        hatchingCoroutine = parentBehaviour.StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.TreeStumpRegrowthTimeSecs, HatchOut));
    }

    public override void Tick()
    {
    }

    public override void Exit(bool disabled)
    {
        parentBehaviour.StopCoroutine(hatchingCoroutine);
        hatchingCoroutine = null;
    }

    private void HatchOut()
    {
        parentBehaviour.HatchOut();
        Owner.TriggerTransition(EggStateEnum.EMPTY);
    }
}
