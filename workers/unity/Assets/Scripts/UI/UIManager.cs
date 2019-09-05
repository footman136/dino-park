using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    
    public bool IsPointerOverGameObject(Vector2 screenPosition)
    {
        //实例化点击事件
        PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        //将点击位置的屏幕坐标赋值给点击事件
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);
 
        List<RaycastResult> results = new List<RaycastResult>();
        //向点击处发射射线
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
 
        return results.Count > 0;
    }
//    ————————————————
//    版权声明：本文为CSDN博主「PassionY」的原创文章，遵循 CC 4.0 BY-SA 版权协议，转载请附上原文出处链接及本声明。
//    原文链接：https://blog.csdn.net/yupu56/article/details/54561553    

    public static PanelCommandMenu CommandMenu { get; set; }

    private PanelSystemTips _systemTips;
    public void SystemTips(string msg, PanelSystemTips.MessageType msgType)
    {
        if (_systemTips == null)
        {
            var go = Resources.Load("UI/PanelSystemTips") as Object;
            if (go!=null)
            {
                var go2 = Instantiate(go, transform) as GameObject;
                _systemTips = go2.GetComponent<PanelSystemTips>();
            }
            else
            {
                Debug.LogError("UI/PanelSystemTips not found!");
            }
        }
        if (_systemTips != null)
        {
            _systemTips.Show(msg, msgType);
        }
    }
}
