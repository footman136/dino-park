using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;

public class EggVanishState : FsmBaseState<EggStateMachine, EggStateEnum>
{
    private readonly EggBehaviour parentBehaviour;
    
    public EggVanishState(EggStateMachine owner, EggBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;
    }

    public override void Enter()
    {
    }

    public override void Tick()
    {
        float deltaTime = Time.time - Owner._startTime;
        if (deltaTime > 5f)
        {
            parentBehaviour.DestroyEgg();
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
