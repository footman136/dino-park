﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Assets.Gamelogic.Utils;
using Dinopark.Npc;
using Improbable.Gdk.Core;

public class DinoStateMachine : FiniteStateMachine<DinoAiFSMState.StateEnum>
{
    public AiData Data;
    public DinoBehaviour _behaviour;
    
    public static Vector3 InvalidPosition = Vector3.one * -9999;
    public float _startTime; // 本状态开始的时间

    public  DinoStateMachine(DinoBehaviour behaviour)
    {
        _behaviour = behaviour;
        
        var idleState = new DinoIdleState(this, behaviour);
        var wanderState = new DinoWanderState(this, behaviour);
        var eatState = new DinoEatState(this, behaviour);
        var runawayState = new DinoRunAwayState(this, behaviour);
        var chaseState = new DinoChaseState(this, behaviour);
        var attackState = new DinoAttackState(this, behaviour);
        var deadState = new DinoDeadState(this, behaviour);
        var hatchState = new DinoHatchState(this, behaviour);
        var lookforfoodState = new DinoLookForFoodState(this, behaviour);
        var eatfoodState = new DinoEatFoodState(this, behaviour);
        var vanishState = new DinoVanishState(this, behaviour); 
        
        var stateList = new Dictionary<DinoAiFSMState.StateEnum, IFsmState>
        {
            { DinoAiFSMState.StateEnum.IDLE, idleState },
            { DinoAiFSMState.StateEnum.EAT, eatState },
            { DinoAiFSMState.StateEnum.WANDER, wanderState },
            { DinoAiFSMState.StateEnum.RUN_AWAY, runawayState },
            { DinoAiFSMState.StateEnum.CHASE, chaseState },
            { DinoAiFSMState.StateEnum.ATTACK, attackState },
            { DinoAiFSMState.StateEnum.DEAD, deadState },
            { DinoAiFSMState.StateEnum.HATCH, hatchState },
            { DinoAiFSMState.StateEnum.LOOK_FOR_FOOD, lookforfoodState },
            { DinoAiFSMState.StateEnum.EAT_FOOD, eatfoodState },
            { DinoAiFSMState.StateEnum.VANISH, vanishState },
        };

        SetStates(stateList);
        
        var allowedTransitions = new Dictionary<DinoAiFSMState.StateEnum, IList<DinoAiFSMState.StateEnum>>();

        allowedTransitions.Add(DinoAiFSMState.StateEnum.IDLE, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.EAT,
            DinoAiFSMState.StateEnum.WANDER,
            DinoAiFSMState.StateEnum.RUN_AWAY,
            DinoAiFSMState.StateEnum.CHASE,
            DinoAiFSMState.StateEnum.ATTACK,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.HATCH,
            DinoAiFSMState.StateEnum.LOOK_FOR_FOOD,
            DinoAiFSMState.StateEnum.EAT_FOOD,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.WANDER, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.RUN_AWAY,
            DinoAiFSMState.StateEnum.CHASE,
            DinoAiFSMState.StateEnum.LOOK_FOR_FOOD,
            DinoAiFSMState.StateEnum.ATTACK,
            DinoAiFSMState.StateEnum.HATCH,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.EAT, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.RUN_AWAY,
            DinoAiFSMState.StateEnum.ATTACK,
            DinoAiFSMState.StateEnum.HATCH,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.EAT_FOOD, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.RUN_AWAY,
            DinoAiFSMState.StateEnum.ATTACK,
            DinoAiFSMState.StateEnum.HATCH,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.LOOK_FOR_FOOD, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.EAT_FOOD,
            DinoAiFSMState.StateEnum.EAT,
            DinoAiFSMState.StateEnum.ATTACK,
            DinoAiFSMState.StateEnum.RUN_AWAY,
            DinoAiFSMState.StateEnum.HATCH,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.RUN_AWAY, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.ATTACK,
            DinoAiFSMState.StateEnum.HATCH,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.CHASE, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.ATTACK,
            DinoAiFSMState.StateEnum.RUN_AWAY,
            DinoAiFSMState.StateEnum.LOOK_FOR_FOOD,
            DinoAiFSMState.StateEnum.CHASE, // 唯一一个地方：可以切换为自己当前的状态
            DinoAiFSMState.StateEnum.HATCH,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.ATTACK, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.EAT,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.HATCH, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.DEAD,
            DinoAiFSMState.StateEnum.ATTACK,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.DEAD, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
            DinoAiFSMState.StateEnum.VANISH,
        });
        allowedTransitions.Add(DinoAiFSMState.StateEnum.VANISH, new List<DinoAiFSMState.StateEnum>
        {
            DinoAiFSMState.StateEnum.IDLE,
        });
        SetTransitions(allowedTransitions);
    }
    
    public void TriggerTransition(DinoAiFSMState.StateEnum newState, EntityId targetEntityId, Vector3 targetPosition)
    {
        if (IsValidTransition(newState))
        {
            var oldState = Data.CurrentAiState;
            Data.CurrentAiState = newState;
            Data.TargetEntityId = targetEntityId;
            Data.TargetPosition = targetPosition.ToVector3f();

            var update = new DinoAiData.Update()
            {
                CurrentAiState = Data.CurrentAiState,
                TargetEntityId = Data.TargetEntityId,
                TargetPosition = Data.TargetPosition
            };
            _behaviour.aiDataWriter.SendUpdate(update);

            _startTime = Time.time;
            TransitionTo(newState);
            if (_behaviour.logChanges)
            {
                Debug.Log("DinoStateMachine: State changed from<" + oldState + "> to<" + newState + ">");
            }
        }
        else
        {
            Debug.LogErrorFormat("DinoStateMachine: Invalid transition from {0} to {1} detected.",
                Data.CurrentAiState, newState);
        }
    }
    protected override void OnEnableImpl()
    {
        Data.CurrentAiState = _behaviour.aiDataWriter.Data.CurrentAiState;
        Data.TargetEntityId = _behaviour.aiDataWriter.Data.TargetEntityId;
        Data.TargetPosition = _behaviour.aiDataWriter.Data.TargetPosition;
        _startTime = Time.time;
    }
    
}
