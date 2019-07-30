using UnityEngine;
using Improbable.Gdk.Subscriptions;

namespace Assets.Gamelogic.Core
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class CharacterModelVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject Model;

        public void SetModelVisibility(bool isVisible)
        {
            Model.SetActive(isVisible);
        }
    }
}
