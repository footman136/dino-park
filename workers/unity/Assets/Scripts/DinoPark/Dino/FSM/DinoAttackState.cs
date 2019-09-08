using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Improbable.Gdk.Core;

public class DinoAttackState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    private float deltaTime = 0;
    public DinoAttackState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
    {
        parentBehaviour = behaviour;
    }
    public override void Enter()
    {
        parentBehaviour.navMeshAgent.SetDestination(parentBehaviour.transform.position);
        deltaTime = 1f;
    }

    public override void Tick()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= 1f)
        { // 每间隔一秒才进攻一次
            deltaTime = 0f;
            DinoBehaviour target;
            if (DinoBehaviour.AllAnimals.TryGetValue(Owner.Data.TargetEntityId.Id, out target))
            {
                float dist = Vector3.Distance(parentBehaviour.transform.position, target.transform.position);
                if (dist >= parentBehaviour.ScriptableAnimalStats.contingencyDistance*2)
                { // 距离太远，离开战斗状态
                    Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
                }
                else if (target.Dead)
                {
                    Owner.TriggerTransition(DinoAiFSMState.StateEnum.EAT, Owner.Data.TargetEntityId, target.transform.position);
                }
                else if (target.IsVanish)
                {
                    Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
                }
                else
                {
                    parentBehaviour.DoAttack(target);
                }
            }
            else
            {
                Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
            }
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
