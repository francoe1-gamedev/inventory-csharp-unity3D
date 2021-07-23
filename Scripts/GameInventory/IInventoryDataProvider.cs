using GameInventory.Interfaces;

namespace GameInventory
{
    public interface IInventoryDataProvider
    {
        void Insert(int x, int y, IItem item);
        void Remove(IItem item);
    }
}
