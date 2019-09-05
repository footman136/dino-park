using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable.Gdk.Subscriptions;
using Assets.Gamelogic.Core;
using Dinopark.Npc;
using Improbable.Gdk.Core;
using Assets.Gamelogic.Utils;
using DinoPark;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.Commands;

[WorkerType(WorkerUtils.UnityClient)]
public class EggVisualizer : MonoBehaviour
{
    [Require] private EggDataReader egg;
    [Require] private EntityId _entityId;

    [Space(), Header("Attributes"), Space(5)]
    [SerializeField] private long _id;
    [SerializeField] private EggTypeEnum _eggType;
    [SerializeField] private float _currentFood;
    [SerializeField] private EggStateEnum _currentState;

    [SerializeField] private GameObject _partGood;
    [SerializeField] private GameObject _partBroken;
    
    // Start is called before the first frame update
    void Start()
    {
        _id = _entityId.Id;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        var update = new EggData.Update
        {
            EggType = egg.Data.EggType,
            CurrentFood = egg.Data.CurrentFood,
            CurrentState = egg.Data.CurrentState
        };
        OnDataChanged(update);
        egg.OnUpdate += OnDataChanged;
    }

    private void OnDisable()
    {
        egg.OnUpdate -= OnDataChanged;
    }


    private void OnDataChanged(EggData.Update update)
    {
        if (update.EggType.HasValue)
        {
            _eggType = update.EggType.Value;
            var renderer = gameObject.GetComponent<MeshRenderer>();
            Color [] color = new Color[3]{Color.black, Color.white, Color.green};
            renderer.material.color = color[(int) _eggType];
        }
        if (update.CurrentFood.HasValue)
        {
            _currentFood = update.CurrentFood.Value;
        }

        if (update.CurrentState.HasValue)
        {
            _currentState = update.CurrentState.Value;
            switch (_currentState)
            {
                case EggStateEnum.GOOD:
                    _partGood.SetActive(true);
                    _partBroken.SetActive(false);
                    break;
                case EggStateEnum.BROKEN:
                    _partGood.SetActive(false);
                    _partBroken.SetActive(true);
                    break;
                case EggStateEnum.EMPTY:
                    _partGood.SetActive(false);
                    _partBroken.SetActive(true);
                    break;
                case EggStateEnum.VANISH:
                    _partGood.SetActive(false);
                    _partBroken.SetActive(true);
                    StartCoroutine(Vanishing());
                    break;
            }
        }
        Debug.Log("EggVisualizer data changed!");
    }
    private IEnumerator Vanishing()
    {
        while (true)
        {
            Vector3 newpos = transform.position;
            newpos.y -= 0.1f;
            transform.position = newpos;
            if (newpos.y <= -5f)
            {
                DestroyEgg();
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    public void DestroyEgg()
    {
//        var linkentity = GetComponent<LinkedEntityComponent>();
//        var request = new WorldCommands.DeleteEntity.Request(linkentity.EntityId);
//        worldCommandSender.SendDeleteEntityCommand(request);
//        //Debug.Log("Server - destroy an egg:"+_entityId);
        // 客户端貌似不能发送WorldCommand
        Destroy(gameObject);
    }

}
