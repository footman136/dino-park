using Dinopark.Plants;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

// Require, WorkerType

namespace Assets.Gamelogic.Tree
{
    public class TreeStateVisualizer : MonoBehaviour
    {
        [Require] private TreeStateReader treeState;
        public TreeStateReader CurrentState { get { return treeState; } }
    }
}
