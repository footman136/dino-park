using System.Collections;
using System.Collections.Generic;
using System.Data;
using DinoPark;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ConnectionStateMachine _stateMachine;
    public ConnectionStateMachine StateMachine => _stateMachine;
    [SerializeField] private UnityClientConnector _clientConnector;
    public UnityClientConnector ClientConnector => _clientConnector;
    public static GameManager Instance { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _stateMachine = new ConnectionStateMachine(this);
        _stateMachine.OnEnable(ConnectionFSMStateEnum.StateEnum.LOGIN);
    }

    private void OnDestroy()
    {
        _stateMachine.OnDisable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public PlayerVisualizer Player { get; set; }
}
