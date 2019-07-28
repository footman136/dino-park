using Assets.Gamelogic.Utils;
using Assets.Gamelogic.Core;
using Dinopark.Plants;
using DinoPark;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
// WorkerUtils
// SimulationSettings
// Quaternion, MathUtils, TimeUtils

// Require, WorkerType

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class TreeModelVisualizer : MonoBehaviour
    {
        [Require] private TreeStateReader treeState;

        [SerializeField] private GameObject HealthyTree;
        [SerializeField] private GameObject Stump;
        [SerializeField] private GameObject BurntTree;
        [SerializeField] private Mesh[] meshes;

        private void OnEnable()
        {
            SetupTreeModel();
            treeState.OnUpdate += (UpdateVisualization);
            ShowTreeModel(treeState.Data.CurrentState);
        }

        private void OnDisable()
        {
            treeState.OnUpdate -= (UpdateVisualization);
        }

        private void SetupTreeModel()
        {
            var treeModel = meshes[(int)treeState.Data.TreeType];
            HealthyTree.GetComponent<MeshFilter>().mesh = treeModel;
        }

        private void UpdateVisualization(TreeState.Update newState)
        {
            ShowTreeModel(newState.CurrentState.Value);
        }

        private void ShowTreeModel(TreeFSMState currentState)
        {
            switch (currentState)
            {
                case TreeFSMState.HEALTHY:
                    StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.TreeExtinguishTimeBuffer, () =>
                    {
                        TransitionTo(HealthyTree);
                    }));
                    break;
                case TreeFSMState.STUMP:
                    StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.TreeCutDownTimeBuffer, () =>
                    {
                        TransitionTo(Stump); 
                    }));
                    break;
                case TreeFSMState.BURNING:
                    StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.TreeIgnitionTimeBuffer, () =>
                    {
                        TransitionTo(HealthyTree);
                    }));
                    break;
                case TreeFSMState.BURNT:
                    TransitionTo(BurntTree);
                    break;
            }
        }

        private void TransitionTo(GameObject newModel)
        {
            HideAllModels();
            newModel.SetActive(true);
        }

        private void HideAllModels()
        {
            HealthyTree.SetActive(false);
            Stump.SetActive(false);
            BurntTree.SetActive(false);
        }
    }
}
