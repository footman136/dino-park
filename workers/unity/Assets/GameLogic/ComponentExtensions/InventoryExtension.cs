using Dinopark.Core;
using UnityEngine;

namespace Assets.Gamelogic.ComponentExtensions
{
    public static class InventoryExtension
    {
        public static bool HasResources(this InventoryReader inventory)
        {
            return inventory.Data.Resources > 0;
        }
        public static bool HasResources(this InventoryWriter inventory)
        {
            return inventory.Data.Resources > 0;
        }

        public static void AddToInventory(this InventoryWriter inventory, int quantity)
        {
            var update = new Inventory.Update()
            {
                Resources = inventory.Data.Resources + quantity
            };
            inventory.SendUpdate(update);
        }

        public static void RemoveFromInventory(this InventoryWriter inventory, int quantity)
        {
            var update = new Inventory.Update()
            {
                Resources = Mathf.Max(0, inventory.Data.Resources - quantity)
            };
            inventory.SendUpdate(update);
        }
    }
}
