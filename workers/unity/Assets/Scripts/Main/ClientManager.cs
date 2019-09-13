using System.Collections;
using System.Collections.Generic;
using System.Data;
using DinoPark;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    // 状态机
    private ConnectionStateMachine _stateMachine;
    public ConnectionStateMachine StateMachine => _stateMachine;
    
    // 客户端网络链接
    [SerializeField] private UnityClientConnector _clientConnector;
    public UnityClientConnector ClientConnector => _clientConnector;
    
    // 登陆器
    [SerializeField] private PlayFabLogin _playFab;
    public PlayFabLogin PlayFab => _playFab;
    
    // 玩家
    public PlayerVisualizer Player { set; get; }

    public static ClientManager Instance { private set; get; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new ConnectionStateMachine(this);
        _stateMachine.OnEnable(ConnectionFSMStateEnum.StateEnum.START);
        if (_playFab == null)
            _playFab = GetComponent<PlayFabLogin>();
    }

    private void OnDestroy()
    {
        _stateMachine.OnDisable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
