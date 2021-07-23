using GameInventory.Interfaces;
using System;
using System.Collections.Generic;

namespace GameInventory
{
    public class SlotExceptionNotItem : Exception { }

    public sealed class Slot
    {
        private List<IItem> _items { get; } = new List<IItem>();
        private Inventory _inventory { get; }

        public IReadOnlyList<IItem> Items => _items;
        public int Amount => _items.Count;
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool HasItem => _items.Count > 0;

        public IItem FirstItem => HasItem ? _items[0] : throw new SlotExceptionNotItem();

        public Slot(Inventory inventory, int x, int y)
        {
            _inventory = inventory;
            X = x;
            Y = y;
        }

        internal bool CompareId(IItem item) => !HasItem || FirstItem.GetId() == item.GetId();

        public void AddItem(IItem item)
        {
            if (!_inventory.ProcessAdd(new ProcessPayload(this, item))) return;
            _items.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            if (!_inventory.ProcessRemove(new ProcessPayload(this, item))) return;
            _items.Remove(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public override string ToString()
        {
            string slot = $"Slot [{X}, {Y}]";
            if (_items.Count == 0) return slot;
            return $"{_items[0].GetName()} x {Amount}";
        }
    }
}
