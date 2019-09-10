using Assets.Gamelogic.Utils;
using Dinopark.Player;
using UnityEngine;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Core;

public class PlayerVisualizer : MonoBehaviour
{
    [Require] private PlayerAttrsReader attrs;
    [Require] private PlayerAttrsCommandSender commandSender;
    [Require] private EntityId entityId;

    [SerializeField] private int _energy;
    public int Energy
    {
        get { return _energy; }
    }

    private PlayerVisualizer _inst;
    public PlayerVisualizer Instance
    {
        get { return _inst; }
    }
    public EntityId Id{
        get { return entityId; }
    } 

    void Awake()
    {
        _inst = this;
    }
        
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Player = this;
        attrs.OnUpdate += OnUpdateAttrs;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnUpdateAttrs(PlayerAttrs.Update update)
    {
        _energy = update.Energy;
        UIManager.Instance.CommandMenu.SetEnergy(_energy);
    }
    
    public void LayEgg(Dinopark.Npc.EggTypeEnum eggType, Vector3 pos)
    {
        LayEggRequest request = new LayEggRequest()
        {
            EggType = eggType,
            EggPosition = pos.ToVector3f(),
            OwnerEntityId = entityId.Id
        };
        commandSender.SendLayEggCommand(entityId, request, OnLayEggResponse);
    }

    private void OnLayEggResponse(PlayerAttrs.LayEgg.ReceivedResponse response)
    {
        if (response.ResponsePayload.HasValue)
        {
            if (response.ResponsePayload.Value.Result)
            {
                // Lay egg succeeded.
                UIManager.Instance.SystemTips("下了个蛋。。。", PanelSystemTips.MessageType.Success);
            }
            else
            {
                string errMsg = "PlayerVisualizer - Lay Egg failed! Error Code:" +
                                response.ResponsePayload.Value.ErrorCode;
                Debug.LogError(errMsg);
                UIManager.Instance.SystemTips(errMsg, PanelSystemTips.MessageType.Error);
            }

            return;
        }
        if(!string.IsNullOrEmpty(response.Message))
        {
            Debug.LogError("PlayerVisualizer - Lay Egg failed! " + response.Message);
            UIManager.Instance.SystemTips(response.Message, PanelSystemTips.MessageType.Error);
        }
    }
}
