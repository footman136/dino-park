using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;

public class GameConnectingState : FsmBaseState<ConnectionStateMachine, ConnectionFSMStateEnum.StateEnum>
{
    private readonly GameManager _game;

    private GameObject _clientWorker;
    private GameObject _panelConnecting;
    
    public GameConnectingState(ConnectionStateMachine owner, GameManager game) : base(owner)
    {
        _game = game;
    }

    public override void Enter()
    {
        _panelConnecting = UIManager.CreatePanel(UIManager.Instance.RootLobby, "", "UI/Lobby/PanelConnecting");
        // clientWorker一启动，就会连接服务器
        _clientWorker = GameManager.Instance.ClientConnector.gameObject;
        _clientWorker.SetActive(true);
    }

    public override void Tick()
    {
    }

    public override void Exit(bool disabled)
    {
        if (_panelConnecting != null)
        {
            UIManager.DestroyPanel(ref _panelConnecting);
        }
    }
}
