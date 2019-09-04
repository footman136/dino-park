using Dinopark.Npc;
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

    void Awake()
    {
        UIManager.CommandMenu = this;
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
            string msgCount = countNow.ToString();
            if (countNow > countOld)
            {
                msgCount = "<color=#7FFF00>" + countNow + "</color>";
            }
            else if (countNow < countOld)
            {
                msgCount = "<color=red>" + countNow + "</color>";
            }
            _counts[i] = countNow;
            _lbCounts[i].text = msgCount.ToString();
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
