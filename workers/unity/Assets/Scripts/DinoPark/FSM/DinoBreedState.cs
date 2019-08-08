﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;

public class DinoBreedState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    public DinoBreedState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;        
    }
    public override void Enter()
    {
        parentBehaviour.navMeshAgent.SetDestination(parentBehaviour.transform.position);
    }

    public override void Tick()
    {
    }

    public override void Exit(bool disabled)
    {
    }
}
