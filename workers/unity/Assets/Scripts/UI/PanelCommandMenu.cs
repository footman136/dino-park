using Dinopark.Npc;
using UnityEngine;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine.UI;

public class PanelCommandMenu : MonoBehaviour
{
    [Require] private WorldCommandSender worldCommandSender;

    [SerializeField] private Text  _lbEnergy;
    [SerializeField] private GameObject _go;
    
    enum CommandType
    {
        CMD_NONE = 0,
        CMD_TREX_EGG = 1,
        CMD_BRACHIO_EGG = 2,
    };

    private CommandType _commandType;
    private Camera _camera;

    void Awake()
    {
        UIManager.CommandMenu = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindObjectOfType<Camera>();
    }

    private void OnDisable()
    {
        Debug.Log("Who did that?");
    }

    // Update is called once per frame
    void Update()
    {
        // 检测如果点击了UI，则直接返回，不进行点击场景的判定
        if (UIManager.Instance.IsPointerOverGameObject(Input.mousePosition))
            return;
        if (Input.GetMouseButtonUp(0))
        {
            EggTypeEnum eggType = EggTypeEnum.NONE;
            switch (_commandType)
            {
                case CommandType.CMD_BRACHIO_EGG:
                    eggType = EggTypeEnum.Brachiosaurus;
                    break;
                case CommandType.CMD_TREX_EGG:
                    eggType = EggTypeEnum.TRex;
                    break;
            }

            if (eggType != EggTypeEnum.NONE)
            {
                var pos = Input.mousePosition;
                Ray ray = new Ray(pos, Vector3.down);
                Ray ray2 = _camera.ScreenPointToRay(Input.mousePosition);
                Debug.Log("Mouse Positin:"+pos+"  ray:"+ray2);
                RaycastHit hitInfo;
                //Physics.Raycast(ray2, out hitInfo, 100, LayerMask.NameToLayer("Ground"));
                Physics.Raycast(ray2, out hitInfo, 100);
                Debug.Log("Hit:" + hitInfo.point);
                if(GameManager.Instance.Player)
                    GameManager.Instance.Player.LayEgg(eggType, hitInfo.point);
            }
        }
    }

    public void OnEggBrachioClicked(GameObject go)
    {
        _commandType = CommandType.CMD_BRACHIO_EGG;
    }

    public void OnEggTRexClicked(GameObject go)
    {
        _commandType = CommandType.CMD_TREX_EGG;
    }

    public void SetEnergy(int energy)
    {
        _lbEnergy.text = energy.ToString();
    }
}
