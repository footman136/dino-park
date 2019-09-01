using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _inst;
    public static UIManager Instance => _inst;

    void Awake()
    {
        _inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static PanelCommandMenu CommandMenu { get; set; }

    private PanelSystemTips _systemTips;
    public void SystemTips(string msg, PanelSystemTips.MessageType msgType)
    {
        if (_systemTips == null)
        {
            var go = Resources.Load("UI/PanelSystemTips") as GameObject;
            if (go!=null)
            {
                var go2 = Instantiate(go, transform);
                _systemTips = go2.GetComponent<PanelSystemTips>();
            }
        }
        if (_systemTips != null)
        {
            _systemTips.Show(msg, msgType);
        }
    }
}
