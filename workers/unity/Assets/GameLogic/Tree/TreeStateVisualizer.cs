using UnityEngine;
using Dinopark.Plants;
using Improbable.Gdk.Subscriptions; // Require, WorkerType

namespace GameLogic.Tree
{
    public class TreeStateVisualizer : MonoBehaviour
    {
        [Require] private TreeStateReader treeState;
        public TreeStateReader CurrentState { get { return treeState; } }
    }
}
