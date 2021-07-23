using GameInventory.Interfaces;
using System;

namespace GameInventory.Processors
{
    public class IdentityItemException : Exception { }

    public class IndentityItemProcessor : IItemProcessor
    {
        bool IItemProcessor.CanAdd(Inventory inventory, ProcessPayload payload)
        {
            if (payload.Slot.HasItem && !payload.Slot.CompareId(payload.Item)) throw new IdentityItemException();
            return true;
        }

        bool IItemProcessor.CanRemove(Inventory inventory, ProcessPayload payload)
        {
            if (!payload.Slot.CompareId(payload.Item)) throw new IdentityItemException();
            return true;
        }
    }
}
