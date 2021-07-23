namespace GameInventory.Interfaces
{
    public interface IItemProcessor
    {
        bool CanAdd(Inventory inventory, ProcessPayload payload);

        bool CanRemove(Inventory inventory, ProcessPayload payload);
    }
}
