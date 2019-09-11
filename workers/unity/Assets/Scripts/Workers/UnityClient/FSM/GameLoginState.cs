using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;

public class GameLoginState : FsmBaseState<ConnectionStateMachine, ConnectionFSMStateEnum.StateEnum>
{
    private readonly GameManager _game;

    private GameObject _panelLogin;

    public GameLoginState(ConnectionStateMachine owner, GameManager game) : base(owner)
    {
        _game = game;
    }

    public override void Enter()
    {
        _panelLogin = UIManager.CreatePanel(UIManager.Instance.RootLobby, "", "UI/Lobby/PanelLogin");
    }

    public override void Tick()
    {
    }

    public override void Exit(bool disabled)
    {
        if (_panelLogin != null)
        {
            UIManager.DestroyPanel(ref _panelLogin);
        }
    }
}
