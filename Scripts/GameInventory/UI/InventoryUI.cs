using GameInventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UnityEngine;

namespace GameInventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Texture2D _slotTexture;

        private Inventory _inventory { get; set; }
        private Queue<InventoryMoveCommand> _commands { get; } = new Queue<InventoryMoveCommand>();

        private void Start()
        {
            _inventory = new Inventory(new InventoryMemoryDataProvider(), 6, 6);
            _inventory.RegisterProcessor(new Processors.IndentityItemProcessor());
            _inventory.RegisterProcessor(new Processors.StackableItemProcessor());

            ObjectLocator.Register(_inventory);
        }

        private Slot _dragSlot { get; set; }
        private Slot _hoverSlot { get; set; }

        private void OnGUI()
        {
            int w = 70;
            int h = 70;
            int space = 0;
            float iconMargin = 20;

            Vector2 screenFix = new Vector2();
            screenFix.x = (Screen.width - ((w + space) * _inventory.Width)) / 2;
            screenFix.y = (Screen.height - ((h + space) * _inventory.Height)) / 2;

            foreach (Slot slot in _inventory)
            {
                Rect rect = new Rect((w + space) * slot.X, (h + space) * slot.Y, w, h);
                rect.position += screenFix;

                GUI.DrawTexture(rect, _slotTexture, ScaleMode.StretchToFill);

                bool inRect() => rect.Contains(Event.current.mousePosition);

                if (slot.HasItem)
                {
                    if (slot.FirstItem is IItemIcon icon)
                    {
                        Rect rectIcon = rect;
                        rectIcon.width -= iconMargin;
                        rectIcon.height -= iconMargin;
                        rectIcon.x += iconMargin / 2;
                        rectIcon.y += iconMargin / 2;

                        GUI.DrawTexture(rectIcon, icon.GetIcon(), ScaleMode.ScaleToFit);

                        Rect rectInfo = rect;
                        rectInfo.x += w / 4;
                        rectInfo.y += h / 4;

                        const string info = "<b><size=20>x {0}</size></b>";
                        GUI.Label(rectInfo, string.Format(info, slot.Amount));
                    }
                    else
                    {
                        GUI.Box(rect, slot.ToString());
                    }
                }

                if (inRect()) _hoverSlot = slot;

                if (inRect() && Event.current.type == EventType.MouseDown && _dragSlot == null)
                {
                    _dragSlot = slot;
                    Event.current.Use();
                }

                if (_dragSlot != null && _dragSlot != slot && inRect() && Event.current.type == EventType.MouseUp)
                {
                    _commands.Enqueue(new InventoryMoveCommand(_dragSlot, slot));
                    _dragSlot = null;
                    Event.current.Use();
                }
            }

            if (_hoverSlot != null)
            {
                DrawHover(_hoverSlot);
                _hoverSlot = null;
            }

            if (_dragSlot != null && Event.current.type == EventType.MouseUp)
            {
                _commands.Enqueue(new InventoryMoveCommand(_dragSlot, null));
                _dragSlot = null;
                Event.current.Use();
            }

            if (_dragSlot != null) DrawDragState();
        }

        private void DrawHover(Slot slot)
        {
            Rect rect = new Rect(Event.current.mousePosition, new Vector2(300, 300));
            rect.x += 20;
            rect.y += 20;
            if (!slot.HasItem) return;

            const string text = "<b><size=15><color=green>{0}</color></size></b>";
            if (slot.Amount == 1 && slot.FirstItem is IItemIndividualInformation individualInfo)            
                GUI.Label(rect, string.Format(text, individualInfo.GetInfo()));            
            else if (slot.Amount > 1 && slot.FirstItem is IItemStackInformation stackInfo)            
                GUI.Label(rect, string.Format(text, stackInfo.GetInfo()));            
        }

        private void Update()
        {
            while(_commands.Count > 0)
            {
                InventoryMoveCommand command = _commands.Dequeue();

                if (command.IsMove)
                {
                    List<IItem> items = command.From.Items.ToList();
                    foreach (IItem item in items)
                    {
                        command.From.RemoveItem(item);
                        _inventory.Insert(command.To.X, command.To.Y, item);
                    }
                }
                else
                {
                    List<IItem> items = command.From.Items.ToList();
                    foreach (IItem item in items)
                    {
                        command.From.RemoveItem(item);
                    }
                }
            }
        }

        private void DrawDragState()
        {
            Rect rect = new Rect(Event.current.mousePosition, new Vector2(100, 20));            
            if (_dragSlot.HasItem && _dragSlot.FirstItem is IItemIcon icon)
            {
                GUI.Box(rect, icon.GetIcon());
                GUI.Label(rect, $"x {_dragSlot.Amount}");
            }
            else
            {
                GUI.Box(rect, _dragSlot.ToString());
            }
        }
    }
}
