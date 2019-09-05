using Dinopark.Player;
using UnityEngine;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Core;
using Assets.Gamelogic.Utils;
using Dinopark.Npc;
using DinoPark;
using Improbable.Gdk.Core.Commands;
using Improbable;

public class PlayerBehaviour : MonoBehaviour
{
    [Require] private PlayerAttrsWriter attrs;

    [Require] private PlayerAttrsCommandReceiver commandReceiver;
    [Require] private WorldCommandSender worldCommandSender;
    
    // 配置文件，先写死再这里，然后变成表格数据
    // Eggs
    public static int CostEnergyEggBrachio = 100;
    public static int CostEnergyEggTRex = 200;
    public static int EnergyOrigin = 95; // 初始能量
    public static int EnergyMax = 1000; // 最大能量
    public static int EnergyGrowth = 5; // 每秒增加多少点

    // Start is called before the first frame update
    void Start()
    {
        commandReceiver.OnLayEggRequestReceived += OnLayEgg;
        
        var update = new PlayerAttrs.Update
        {
            Energy =  EnergyOrigin,
        };
        attrs.SendUpdate(update);
        InvokeRepeating("EnergyGrowing", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnergyGrowing()
    {
        var update = new PlayerAttrs.Update
        {
            Energy =  attrs.Data.Energy + EnergyGrowth,
        };
        attrs.SendUpdate(update);
    }

    void OnLayEgg(PlayerAttrs.LayEgg.ReceivedRequest request)
    {
        Coordinates coords = request.Payload.EggPosition.ToUnityVector().ToCoordinates();
        EggTypeEnum eggType = request.Payload.EggType;
        // 得到下蛋的能量消耗
        int costEnergy = 0;
        switch (eggType)
        {
            case EggTypeEnum.Brachiosaurus:
                costEnergy = CostEnergyEggBrachio;
                break;
            case EggTypeEnum.TRex:
                costEnergy = CostEnergyEggTRex;
                break;
        }

        if (attrs.Data.Energy < costEnergy)
        {// 能量不夠不能下蛋
            string errMsg = "Energy is not enough. (" + attrs.Data.Energy + "/" + costEnergy+")";
            commandReceiver.SendLayEggFailure(request.RequestId, errMsg);
            return;
        }
        // 更新能量消耗
        var update = new PlayerAttrs.Update
        {
            Energy =  attrs.Data.Energy - costEnergy,
        };
        attrs.SendUpdate(update);
        // 发送创建实体消息
        var exampleEntity = EntityTemplateFactory.CreateEggTemplate(coords, 0, eggType);
        var request1 = new WorldCommands.CreateEntity.Request(exampleEntity);
        request1.Context = request.RequestId;
        worldCommandSender.SendCreateEntityCommand(request1, OnCreateEggResponse);
    }
    private void OnCreateEggResponse(WorldCommands.CreateEntity.ReceivedResponse response)
    {
        if (response.StatusCode != Improbable.Worker.CInterop.StatusCode.Success)
        {// 创建实体失败
            Debug.LogError("PlayerBehaviour - OnCreateEggResponse() failed! " +response.Message);
            string errMsg = "Send CreateEntigyCommand failed - " + response.Message;
            long requestId = (long) response.Context;
            commandReceiver.SendLayEggFailure(requestId, errMsg);
            return;
        }
        if (response.EntityId.HasValue)
        {// 返回下蛋成功消息
            var entityId = response.EntityId.Value;
            //Debug.Log("Server - new egg created:"+entityId);
            long requestId = (long) response.Context;
            var response1 = new LayEggResponse
            {
                Result = true,
                ErrorCode = 0
            };
            commandReceiver.SendLayEggResponse(requestId, response1);
        }

    }
}
