using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Improbable.Gdk.Core;

/// <summary>
/// 客户端管理游戏状态的状态机 
/// </summary>
public class ConnectionStateMachine : FiniteStateMachine<ConnectionFSMStateEnum.StateEnum>
{
    private GameManager _game;
    public ConnectionFSMStateEnum.StateEnum CurrentState { private set; get; }
    public float _startTime; // 本状态开始的时间
    [SerializeField] private bool logChanges = false;
    
    ConnectionStateMachine(GameManager game)
    {
        _game = game;
            
        var loginState = new GameLoginState(this, _game);
        var connectingState = new GameConnectingState(this, _game);
        var connectedState = new GameConnectedState(this, _game);
        var disconnectedState = new GameDisconnectedState(this, _game);
        var resultState = new GameResultState(this, _game);
        var playingState = new GamePlayingState(this, _game);

        var stateList = new Dictionary<ConnectionFSMStateEnum.StateEnum, IFsmState>
        {
            {ConnectionFSMStateEnum.StateEnum.LOGIN, loginState},
            {ConnectionFSMStateEnum.StateEnum.CONNECTING, connectingState},
            {ConnectionFSMStateEnum.StateEnum.CONNECTED, connectedState},
            {ConnectionFSMStateEnum.StateEnum.DISCONNECTED, disconnectedState},
            {ConnectionFSMStateEnum.StateEnum.RESULT, resultState},
            {ConnectionFSMStateEnum.StateEnum.PLAYING, playingState},
        };
        SetStates(stateList);
        
        var allowedTransitions = new Dictionary<ConnectionFSMStateEnum.StateEnum, IList<ConnectionFSMStateEnum.StateEnum>>();

        allowedTransitions.Add(ConnectionFSMStateEnum.StateEnum.LOGIN, new List<ConnectionFSMStateEnum.StateEnum>
        {
            ConnectionFSMStateEnum.StateEnum.CONNECTING,
        });
        allowedTransitions.Add(ConnectionFSMStateEnum.StateEnum.CONNECTING, new List<ConnectionFSMStateEnum.StateEnum>
        {
            ConnectionFSMStateEnum.StateEnum.CONNECTED,
            ConnectionFSMStateEnum.StateEnum.DISCONNECTED,
            ConnectionFSMStateEnum.StateEnum.LOGIN,
        });
        allowedTransitions.Add(ConnectionFSMStateEnum.StateEnum.CONNECTED, new List<ConnectionFSMStateEnum.StateEnum>
        {
            ConnectionFSMStateEnum.StateEnum.PLAYING,
            ConnectionFSMStateEnum.StateEnum.LOGIN,
        });
        allowedTransitions.Add(ConnectionFSMStateEnum.StateEnum.DISCONNECTED, new List<ConnectionFSMStateEnum.StateEnum>
        {
            ConnectionFSMStateEnum.StateEnum.LOGIN,
        });
        allowedTransitions.Add(ConnectionFSMStateEnum.StateEnum.RESULT, new List<ConnectionFSMStateEnum.StateEnum>
        {
            ConnectionFSMStateEnum.StateEnum.LOGIN,
        });
        allowedTransitions.Add(ConnectionFSMStateEnum.StateEnum.PLAYING, new List<ConnectionFSMStateEnum.StateEnum>
        {
            ConnectionFSMStateEnum.StateEnum.LOGIN,
        });
        SetTransitions(allowedTransitions);
    }
    public void TriggerTransition(ConnectionFSMStateEnum.StateEnum newState)
    {
        if (IsValidTransition(newState))
        {
            var oldState = CurrentState; 
            CurrentState = newState;

            _startTime = Time.time;
            TransitionTo(newState);
            if (logChanges)
            {
                Debug.Log("DinoStateMachine: State changed from<" + oldState + "> to<" + newState + ">");
            }
        }
        else
        {
            Debug.LogErrorFormat("DinoStateMachine: Invalid transition from {0} to {1} detected.",
                CurrentState, newState);
        }
    }
    protected override void OnEnableImpl()
    {
        _startTime = Time.time;
    }
    
}
