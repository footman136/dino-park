using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;

public class DinoVanishState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;

    public DinoVanishState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;
    }

    public override void Enter()
    {
        parentBehaviour.navMeshAgent.SetDestination(parentBehaviour.transform.position);
    }

    public override void Tick()
    {
        float deltaTime = Time.time - Owner._startTime;
        if (deltaTime > 30f)
        {
            parentBehaviour.DestroyDino();
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
