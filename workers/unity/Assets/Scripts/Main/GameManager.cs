using System.Collections;
using System.Collections.Generic;
using System.Data;
using DinoPark;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ConnectionStateMachine _stateMachine;
    [SerializeField] private UnityClientConnector _clientConnector;
    public UnityClientConnector ClientConnector
    {
        get { return _clientConnector; }
    } 
    public static GameManager Instance { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _stateMachine.TriggerTransition(ConnectionFSMStateEnum.StateEnum.START);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public PlayerVisualizer Player { get; set; }
}
