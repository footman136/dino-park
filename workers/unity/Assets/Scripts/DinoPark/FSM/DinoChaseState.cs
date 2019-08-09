using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Improbable.Gdk.Core;

public class DinoChaseState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    public DinoChaseState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;        
    }
    public override void Enter()
    {
        parentBehaviour.navMeshAgent.SetDestination(Owner.Data.TargetPosition.ToUnityVector());
        parentBehaviour.navMeshAgent.speed = parentBehaviour.ScriptableAnimalStats.runSpeed;
    }

    public override void Tick()
    {
        // 时间到，不追了
        float deltaTime = Time.time - Owner._startTime;
        if (deltaTime > parentBehaviour.ScriptableAnimalStats.stamina)
        {
            if (parentBehaviour.logChanges)
            {
                Debug.Log("Time's up.");
            }
            Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
            return;
        }
        
        int arrived = 0;
        DinoBehaviour prey = null;
        if (DinoBehaviour.AllAnimals.TryGetValue(Owner.Data.TargetEntityId.Id, out prey))
        {
            if (prey.Dead())
            {// 敌人已经死亡
                Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
            }
            else
            {
                float dist = Vector3.Distance(parentBehaviour.transform.position, prey.transform.position);
                if (dist < parentBehaviour.ScriptableAnimalStats.contingencyDistance)
                {
                    Owner.TriggerTransition(DinoAiFSMState.StateEnum.ATTACK, Owner.Data.TargetEntityId, prey.transform.position);
                    // 这里不让对方也进入“攻击状态”，为了保证线程安全，尽量只对自己的实例进行操作，对方挨打后自己设置攻击状态。
                    // 详见DinoBehaviour.cs里的OnAttack()函数
                }
                else if (parentBehaviour.navMeshAgent.remainingDistance < parentBehaviour.navMeshAgent.stoppingDistance)
                {// 重新寻路，注意，这里直接重新寻路，而不是切换到休闲，然后再寻找猎物，主要是为了让霸王龙更加容易追到猎物
                    parentBehaviour.navMeshAgent.SetDestination(prey.transform.position);
                    parentBehaviour.navMeshAgent.speed = parentBehaviour.ScriptableAnimalStats.runSpeed;
                    parentBehaviour.stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.CHASE, prey._entityId, prey.transform.position);
                }
            }
        }
        else
        {// 敌人已经消失
            Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
