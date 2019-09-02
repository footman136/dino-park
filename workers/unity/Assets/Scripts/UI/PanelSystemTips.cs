using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PanelSystemTips : MonoBehaviour
{
    [SerializeField] private Text _lbMsg;
    [SerializeField] private Animation _ani;
    [SerializeField] private AnimationClip _clip;

    public enum MessageType
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        Success = 4,
        Important = 5,
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(string msg, MessageType msgType)
    {
        _lbMsg.text = msg;
        Color color = Color.white;
        switch (msgType)
        {
            case MessageType.Info:
                color = Color.white;
                break;
            case MessageType.Warning:
                color = Color.yellow;
                break;
            case MessageType.Error:
                color = Color.red;
                break;
            case MessageType.Success:
                color = Color.green;
                break;
            case MessageType.Important:
                color = Color.blue;
                break;
        }

        _lbMsg.color = color;
        _ani.Stop();
        _ani.clip = _clip;
        _ani.Play();
    }
    
}
