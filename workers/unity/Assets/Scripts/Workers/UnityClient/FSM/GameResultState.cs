﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;

public class GameResultState : FsmBaseState<ConnectionStateMachine, ConnectionFSMStateEnum.StateEnum>
{
    private readonly ClientManager _game;

    public GameResultState(ConnectionStateMachine owner, ClientManager game) : base(owner)
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
