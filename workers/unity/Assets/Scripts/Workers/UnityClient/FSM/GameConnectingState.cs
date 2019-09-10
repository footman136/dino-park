using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;

public class GameConnectingState : FsmBaseState<ConnectionStateMachine, ConnectionFSMStateEnum.StateEnum>
{
    private readonly GameManager _game;

    public GameConnectingState(ConnectionStateMachine owner, GameManager game) : base(owner)
    {
        _game = game;
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
