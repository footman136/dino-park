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
        int arrived = 0;
        if (parentBehaviour.navMeshAgent.remainingDistance < parentBehaviour.navMeshAgent.stoppingDistance)
        {
            arrived = 1;
        }

        float deltaTime = Time.time - Owner._startTime;
        if (deltaTime > parentBehaviour.ScriptableAnimalStats.stamina)
        {
            arrived = 2;
            if (parentBehaviour.logChanges)
            {
                Debug.Log("Time's up.");
            }
        }

        DinoBehaviour target = null;
        if (DinoBehaviour.AllAnimals.TryGetValue(Owner.Data.TargetEntityId.Id, out target))
        {
            float dist = Vector3.Distance(parentBehaviour.transform.position, target.transform.position);
            if (dist < parentBehaviour.ScriptableAnimalStats.contingencyDistance)
            {
                arrived = 3;
            }
        }

        if (arrived == 3 && target != null)
        {   // 让自己进入“攻击”状态
            Owner.TriggerTransition(DinoAiFSMState.StateEnum.ATTACK, Owner.Data.TargetEntityId, target.transform.position);
            // 让对方进入“被攻击”状态
            if (!target.Dead() && target.stateMachine.CurrentState != DinoAiFSMState.StateEnum.BE_ATTACK)
            {
                target.stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.BE_ATTACK, parentBehaviour._entityId,
                    parentBehaviour.transform.position);
            }
        }
        else if(arrived>0)
        {
            Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
