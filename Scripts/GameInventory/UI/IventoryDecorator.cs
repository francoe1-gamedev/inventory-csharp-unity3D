using GameInventory.Interfaces;

namespace GameInventory.UI
{
    public static class IventoryDecorator
    {
        public static void Insert(this Inventory inventory, int x, int y, IItem item, int recursiveAttemp = 0)
        {
            if (recursiveAttemp >= inventory.Lenght) throw new System.Exception("Sin espacio en el inventario");

            int GetMaxStackItem(Slot slot) => slot.HasItem && slot.FirstItem is IItemStackable stackable ? stackable.MaxStackableItem() : 1;
            bool HasSpace(Slot slot) => GetMaxStackItem(slot) > slot.Amount;

            Slot slot = inventory.GetSlot(x, y);

            {
                if (!slot.CompareId(item))
                {
                    (int new_x, int new_y) = inventory.GetNextPosition(x, y);
                    inventory.Insert(new_x, new_y, item, recursiveAttemp + 1);
                    return;
                }

                if (!HasSpace(slot))
                {
                    (int new_x, int new_y) = inventory.GetNextPosition(x, y);
                    inventory.Insert(new_x, new_y, item, recursiveAttemp + 1);
                    return;
                }

                slot.AddItem(item);
            }
        }
    }
}
