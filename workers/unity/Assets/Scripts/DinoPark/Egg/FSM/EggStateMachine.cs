using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dinopark.Npc;
using Assets.Gamelogic.FSM;
using Improbable.Gdk.Core;

public class EggStateMachine : FiniteStateMachine<EggStateEnum>
{
    public EggStateEnum curState;
    public EggBehaviour _behaviour;
    public float _startTime; // 本状态开始的时间
    
    public  EggStateMachine(EggBehaviour behaviour)
    {
        _behaviour = behaviour;
        
        var goodState = new EggGoodState(this, behaviour);
        var brokenState = new EggBrokenState(this, behaviour);
        var emptyState = new EggEmptyState(this, behaviour);
        var vanishState = new EggVanishState(this, behaviour);
        
        var stateList = new Dictionary<EggStateEnum, IFsmState>
        {
            { EggStateEnum.GOOD, goodState },
            { EggStateEnum.BROKEN, brokenState },
            { EggStateEnum.EMPTY, emptyState },
            { EggStateEnum.VANISH, vanishState },
        };

        SetStates(stateList);
        
        var allowedTransitions = new Dictionary<EggStateEnum, IList<EggStateEnum>>();

        allowedTransitions.Add(EggStateEnum.GOOD, new List<EggStateEnum>
        {
            EggStateEnum.NONE,
            EggStateEnum.VANISH,
            EggStateEnum.BROKEN,
            EggStateEnum.EMPTY,
        });
        allowedTransitions.Add(EggStateEnum.BROKEN, new List<EggStateEnum>
        {
            EggStateEnum.VANISH,
        });
        allowedTransitions.Add(EggStateEnum.EMPTY, new List<EggStateEnum>
        {
            EggStateEnum.VANISH,
        });
        allowedTransitions.Add(EggStateEnum.VANISH, new List<EggStateEnum>
        {
            EggStateEnum.NONE,
        });
        SetTransitions(allowedTransitions);
    }
        
    public void TriggerTransition(EggStateEnum newState)
    {
        if (IsValidTransition(newState))
        {
            var oldState = curState; 
            curState = newState;

            var update = new EggData.Update()
            {
                CurrentState = curState,
            };
            _behaviour.egg.SendUpdate(update);

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
                curState, newState);
        }
    }
    protected override void OnEnableImpl()
    {
        curState = _behaviour.egg.Data.CurrentState;
        _startTime = Time.time;
    }
}
