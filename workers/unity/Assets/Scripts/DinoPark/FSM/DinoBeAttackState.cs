using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Improbable.Gdk.Core;

public class DinoBeAttackState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    private float deltaTime = 0;
    public DinoBeAttackState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;
    }
    public override void Enter()
    {
        parentBehaviour.navMeshAgent.SetDestination(parentBehaviour.transform.position);
        deltaTime = 0f;
    }

    // 因为是被攻击者掉血，所以攻击计算放在被攻击者身上进行，保证线程安全
    public override void Tick()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= 1f)
        { // 每间隔一秒才进攻一次
            deltaTime = 0f;
            DinoBehaviour attacker;
            if (DinoBehaviour.AllAnimals.TryGetValue(Owner.Data.TargetEntityId.Id, out attacker))
            {
                float dist = Vector3.Distance(parentBehaviour.transform.position, attacker.transform.position);
                if (dist >= parentBehaviour.ScriptableAnimalStats.contingencyDistance)
                { // 距离太远，离开战斗状态
                    Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
                }
                else
                {
                    parentBehaviour.TakeDamage(attacker.ScriptableAnimalStats.power);
                }
            }
            else
            {
                parentBehaviour.TakeDamage(attacker.ScriptableAnimalStats.power);
            }
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
