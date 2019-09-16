using Assets.Gamelogic.Tree;
using Dinopark.Npc;
using Dinopark.Plants;
using UnityEngine;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using LowPolyAnimalPack;
using UnityEngine.UI;

public class PanelCommandMenu : MonoBehaviour
{
    [Require] private WorldCommandSender worldCommandSender;

    [SerializeField] private Text _lbEnergy;
    [SerializeField] private Text [] _lbCounts; 

    private int [] _counts;
    
    enum CommandType
    {
        CMD_NONE = 0,
        CMD_TREX_EGG = 1,
        CMD_BRACHIO_EGG = 2,
    };

    private CommandType _commandType;
    private Camera _camera;
#region 系统函数
    void Awake()
    {
        UIManager.Instance.CommandMenu = this;
        _counts = new int[(int) AnimalManager.ANIMAL_TYPE.COUNT];
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
        UpdateCounts();
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
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                Debug.Log("Mouse Position:"+pos+"  ray:"+ray);
                RaycastHit hitInfo;
                bool ret = Physics.Raycast(ray, out hitInfo, 200);
                if (!ret)
                {
                    UIManager.Instance.SystemTips("鼠标检测地面失败！", PanelSystemTips.MessageType.Error);                    
                }
                else
                {
                    Debug.Log("Hit:" + hitInfo.point);
                    if(ClientManager.Instance.Player)
                        ClientManager.Instance.Player.LayEgg(eggType, hitInfo.point, ClientManager.Instance.TokenId);
                }
            }
        }
    }
#endregion

#region 内部函数
    private const float TIME_DELAY = 1f; 
    private float _lastTime = 0;
    private void UpdateCounts()
    {
        _lastTime += Time.deltaTime;
        if (_lastTime < TIME_DELAY)
        {
            return;
        }

        _lastTime = 0;
        for (int i = 0; i < (int) AnimalManager.ANIMAL_TYPE.COUNT; ++i)
        {
            int countOld = _counts[i];
            int countNow = AnimalManager.Instance.Roots[i].childCount;
            string msgCount = "";
            if (i == (int) AnimalManager.ANIMAL_TYPE.PLAYER)
            {
                msgCount = $"【{ClientManager.Instance.Account}】";
            }
            else if (i == (int) AnimalManager.ANIMAL_TYPE.TREE)
            {
                countNow = CalcTreeAliveCount(AnimalManager.Instance.Roots[i]);
                msgCount = countNow.ToString();
            }
            else if (i == (int) AnimalManager.ANIMAL_TYPE.TREX || i == (int) AnimalManager.ANIMAL_TYPE.BRACHIO)
            {
                int myCount = 0;
                int enemyCount = 0;
                CalcDinoTeamCount(AnimalManager.Instance.Roots[i], out myCount, out enemyCount);
                msgCount = $"{countNow}({myCount}:{enemyCount})";
            }
            else
            {
                msgCount = countNow.ToString();                
            }
            _lbCounts[i].fontSize = 14;
            if (countNow > countOld)
            {
                msgCount = "<color=#22BB22>" + msgCount + "</color>";
                _lbCounts[i].fontSize = 18;
            }
            else if (countNow < countOld)
            {
                msgCount = "<color=red>" + msgCount + "</color>";
                _lbCounts[i].fontSize = 18;
            }
            _counts[i] = countNow;
            _lbCounts[i].text = msgCount.ToString();
        }
    }

    private int CalcTreeAliveCount(Transform root)
    {
        int countAlive = 0;
        for (int i = 0; i < root.childCount; ++i)
        {
            TreeModelVisualizer tsv = root.GetChild(i).GetComponent<TreeModelVisualizer>();
            if (tsv != null && tsv.CurrentState.Data.CurrentState == TreeFSMState.HEALTHY)
            {
                countAlive++;
            }
        }

        return countAlive;
    }

    private void CalcDinoTeamCount(Transform root, out int myCount, out int enemyCount)
    {
        myCount = 0;
        enemyCount = 0;
        for (int i = 0; i < root.childCount; ++i)
        {
            DinoVisualizer dv = root.GetChild(i).GetComponent<DinoVisualizer>();
            if (dv != null)
            {
                long tokenId = dv.attrsReader.Data.OwnerTokenId;
                if (tokenId != 0)
                {
                    if (tokenId == ClientManager.Instance.TokenId)
                    {
                        myCount++;
                    }
                    else
                    {
                        enemyCount++;
                    }
                }
            }
        }
    }
#endregion

#region 命令响应    
    public void OnEggBrachioClicked(GameObject go)
    {
        _commandType = CommandType.CMD_BRACHIO_EGG;
    }

    public void OnEggTRexClicked(GameObject go)
    {
        _commandType = CommandType.CMD_TREX_EGG;
    }
#endregion
    
#region 操作
    public void SetEnergy(int energy)
    {
        _lbEnergy.text = "Energy:" + energy.ToString();
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
#endregion
}
