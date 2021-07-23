using GameInventory.Interfaces;
using System;

namespace GameInventory.Processors
{
    public class StackableItemException : Exception 
    {
        public StackableItemException(string message) : base(message) { }
    }

    public class StackableItemProcessor : IItemProcessor
    {
        bool IItemProcessor.CanAdd(Inventory inventory, ProcessPayload payload)
        {
            if (payload.Item is IItemStackable stackable && stackable.MaxStackableItem() <= payload.Slot.Amount) throw new StackableItemException($"Sin Espacio, maximo {stackable.MaxStackableItem()}");
            return true;
        }

        bool IItemProcessor.CanRemove(Inventory inventory, ProcessPayload payload)
        {
            return true;
        }
    }
}
