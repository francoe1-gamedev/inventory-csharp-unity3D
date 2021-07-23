using GameInventory.Interfaces;

namespace GameInventory
{
    public struct ProcessPayload
    {
        public Slot Slot { get; }
        public IItem Item { get; }

        public ProcessPayload(Slot slot, IItem item)
        {
            Slot = slot;
            Item = item;
        }
    }
}
