using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform _rootLobby;
    [SerializeField] private Transform _rootInGame;
    
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
#region 杂项    
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

    private PanelSystemTips _systemTips;
    public void SystemTips(string msg, PanelSystemTips.MessageType msgType)
    {
        if (_systemTips == null)
        {
            var go = Resources.Load("UI/PanelSystemTips");
            if (go!=null)
            {
                var go2 = Instantiate(go, transform) as GameObject;
                if (go2 != null)
                {
                    _systemTips = go2.GetComponent<PanelSystemTips>();
                }
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
#endregion

#region 游戏内界面
    public PanelCommandMenu CommandMenu { get; set; }

    public void ShowCommandMenu( bool show)
    {
        if (CommandMenu == null)
        {
            var go = Resources.Load("UI/InGame/PanelCommandMenu");
            if (go != null)
            {
                var go2 = Instantiate(go, _rootInGame) as GameObject;
                if (go2 != null)
                {
                    CommandMenu = go2.GetComponent<PanelCommandMenu>();
                }
            }
        }

        if (CommandMenu != null)
        {
            CommandMenu.Show(show);
        }
    }
#endregion
    
#region 大厅界面

    public void ShowLobbyMenu(bool show)
    {
        
    }
#endregion
}
