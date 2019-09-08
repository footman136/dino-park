using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Improbable.Gdk.Core;

public class DinoEatState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    private float deltaTime = 0;
    public DinoEatState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
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
        {
            deltaTime = 0;
            if (parentBehaviour.attrsWriter.Data.CurrentFood + parentBehaviour.ScriptableAnimalStats.liveCost >=
                parentBehaviour.attrsWriter.Data.MaxFood)
            { // 吃饱了，不吃了，离开本状态
                Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
                return;
            }
            
            DinoBehaviour corpse = null;
            if (DinoBehaviour.AllAnimals.TryGetValue(Owner.Data.TargetEntityId.Id, out corpse))
            {
                if (corpse.IsVanish)
                {
                    Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
                }
                else
                {
                    if (!corpse.Dead)
                        Debug.LogError("DinoEatState - Cannot eat live animals! Id:"+corpse._entityId+" state:"+corpse.stateMachine.CurrentState);
                    parentBehaviour.DoEat(corpse);
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
