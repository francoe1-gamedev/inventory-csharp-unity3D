using GameInventory.Interfaces;
using UnityEngine;

namespace GameInventory.UI
{
    [CreateAssetMenu(fileName = "Stackable Item", menuName = "Items/Stackable")]
    public class StackableItem : BaseItem, IItem, IItemStackable, IItemStackInformation
    {
        [Header("Stack")]
        [SerializeField] private int _maxStackable = 16;
        [SerializeField] private string _stackInformation = "";

        string IItemStackInformation.GetInfo() => _stackInformation;

        int IItemStackable.MaxStackableItem() => _maxStackable;
    }
}
