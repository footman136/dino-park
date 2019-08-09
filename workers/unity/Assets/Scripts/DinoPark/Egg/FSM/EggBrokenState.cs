using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;


public class EggBrokenState : FsmBaseState<EggStateMachine, EggStateEnum>
{
    private readonly EggBehaviour parentBehaviour;
    
    public EggBrokenState(EggStateMachine owner, EggBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;
    }

    public override void Enter()
    {
    }

    public override void Tick()
    {
    }

    public override void Exit(bool disabled)
    {
    }
}
