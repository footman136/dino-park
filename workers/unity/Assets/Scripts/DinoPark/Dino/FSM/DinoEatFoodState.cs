using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Npc;
using Improbable.Gdk.Core;
using Assets.Gamelogic.Tree;

public class DinoEatFoodState : FsmBaseState<DinoStateMachine, DinoAiFSMState.StateEnum>
{
    private readonly DinoBehaviour parentBehaviour;
    private float deltaTime = 0;
    public DinoEatFoodState(DinoStateMachine owner, DinoBehaviour behaviour) : base(owner)
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
                parentBehaviour.ScriptableAnimalStats.foodStorage)
            { // 吃饱了，不吃了，离开本状态
                Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
                return;
            }
            
            TreeBehaviour aTree = null;
            if (TreeBehaviour.AllTrees.TryGetValue(Owner.Data.TargetEntityId.Id, out aTree))
            {
                if (!aTree.IsHavestable())
                {
                    Owner.TriggerTransition(DinoAiFSMState.StateEnum.IDLE, new EntityId(), DinoStateMachine.InvalidPosition);
                }
                else
                {
                    parentBehaviour.HarvestFood(aTree);
                }
            }
        }
    }

    public override void Exit(bool disabled)
    {
    }
}
