//using Assets.Gamelogic.UI;

using Dinopark.Life;
using Assets.Gamelogic.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;
//using Improbable.Team;
// Require, WorkerType

// WorkerUtils

namespace Assets.Gamelogic.Life
{
    [WorkerType(WorkerUtils.UnityClient)]
    public class HealthClientVisualizer : MonoBehaviour
    {
        [Require] private HealthReader health;
        //[Require] private TeamAssignment.Reader teamAssigment;

        public int CurrentHealth { get { return health.Data.CurrentHealth; } }
        public int MaxHealth { get { return health.Data.MaxHealth; } }
        private GameObject entityInfoCanvasInstance;
        //private EntityHealthPanelController entityHealthPanelController;

        private void Awake()
        {
            //entityInfoCanvasInstance = (GameObject) Instantiate(ResourceRegistry.EntityInfoCanvasPrefab, transform);
            Collider modelCollider = GetComponent<Collider>();
            if (modelCollider == null)
            {
                modelCollider = GetComponentInChildren<Collider>();
            }
            entityInfoCanvasInstance.transform.localPosition = (modelCollider != null) ? Vector3.up * (modelCollider.bounds.size.y + 1.5f) : Vector3.up * 3f;
            //entityHealthPanelController = entityInfoCanvasInstance.GetComponent<EntityHealthPanelController>();
        }

        private void OnEnable()
        {
            UpdateEntityHealthPanel();
            //entityHealthPanelController.SetHealthColorFromTeam(teamAssigment.Data.teamId);
            health.OnUpdate += (OnComponentUpdated);
        }

        private void OnDisable()
        {
            health.OnUpdate -= (OnComponentUpdated);
        }

        private void OnComponentUpdated(Health.Update update)
        {
            UpdateEntityHealthPanel();
        }

        private void UpdateEntityHealthPanel()
        {
            if (CurrentHealth == MaxHealth)
            {
                //entityHealthPanelController.Hide();
            }
            else
            {
                //entityHealthPanelController.Show();
                //entityHealthPanelController.SetHealth(((float)CurrentHealth) / MaxHealth);
            }
        }
    }
}
