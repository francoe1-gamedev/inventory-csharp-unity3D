using GameInventory.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameInventory
{
    public class Inventory : IEnumerable<Slot>
    {
        private Slot[,] _slots { get; }
        private List<IItemProcessor> _processors { get; } = new List<IItemProcessor>();
        private IInventoryDataProvider _dataProvider { get; }

        public int Lenght => _slots.GetLength(0) * _slots.GetLength(1);
        public int Width => _slots.GetLength(0);
        public int Height => _slots.GetLength(1);

        public Inventory(IInventoryDataProvider dataProvider, int w, int h)
        {
            _dataProvider = dataProvider;
            _slots = new Slot[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    _slots[x, y] = new Slot(this, x, y);
        }

        public void RegisterProcessor(IItemProcessor processor)
        {
            _processors.Add(processor);
        }

        public Slot GetSlot(int x, int y) => _slots[x, y];

        public bool ProcessAdd(ProcessPayload payload)
        {
            foreach (IItemProcessor processor in _processors) if (!processor.CanAdd(this, payload)) return false;
            _dataProvider.Insert(payload.Slot.X, payload.Slot.Y, payload.Item);
            return true;
        }

        public bool ProcessRemove(ProcessPayload payload)
        {
            foreach (IItemProcessor processor in _processors) if (!processor.CanRemove(this, payload)) return false;
            _dataProvider.Remove(payload.Item);
            return true;
        }

        private IEnumerable<Slot> IterateSlots()
        {
            for (int x = 0; x < _slots.GetLength(0); x++)
                for (int y = 0; y < _slots.GetLength(1); y++)
                    yield return _slots[x, y];
        }

        IEnumerator<Slot> IEnumerable<Slot>.GetEnumerator() => IterateSlots().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => IterateSlots().GetEnumerator();

        public (int x, int y) GetNextPosition(int x, int y)
        {
            x ++;
            if (x >= _slots.GetLength(0))
            {
                x = 0;
                y++;
            }

            if (y >= _slots.GetLength(1))
            {
                y = 0;
                x++;
            }
            return (x , y);
        }

        public IEnumerable<Slot> Query(Predicate<Slot> predicate)
        {
            foreach (Slot slot in IterateSlots())
                if (predicate(slot))
                    yield return slot;
        }

        public Slot QueryFirst(Predicate<Slot> predicate)
        {
            foreach (Slot slot in IterateSlots())
                if (predicate(slot))
                   return slot;
            return default;
        }
    }
}
