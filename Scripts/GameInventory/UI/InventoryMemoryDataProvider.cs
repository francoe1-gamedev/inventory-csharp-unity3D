using GameInventory.Interfaces;
using System;
using System.Collections.Generic;

namespace GameInventory.UI
{
    public class InventoryMemoryDataProvider : IInventoryDataProvider
    {
        private List<IItem> _items { get; }

        public InventoryMemoryDataProvider()
        {
            _items = new List<IItem>();
        }

        void IInventoryDataProvider.Insert(int x, int y, IItem item)
        {
            if (_items.Contains(item)) throw new Exception("Este elemento ya esta en el inventario");
            _items.Add(item);
        }

        void IInventoryDataProvider.Remove(IItem item)
        {
            _items.Remove(item);
        }
    }
}
