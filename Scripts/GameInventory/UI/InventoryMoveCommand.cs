namespace GameInventory.UI
{
    public class InventoryMoveCommand
    {
        public Slot From { get; private set; }
        public Slot To { get; private set; }

        public bool IsMove => From != null && To != null;

        public InventoryMoveCommand(Slot dragSlot, Slot slot)
        {
            From = dragSlot;
            To = slot;
        }
    }
}
