using GameInventory.Interfaces;
using UnityEngine;

namespace GameInventory.UI
{
    [CreateAssetMenu(fileName = "Base", menuName = "Items/Base")]
    public class BaseItem : ScriptableObject, IItem, IItemIcon, IItemIndividualInformation
    {
        [Header("Info")]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Texture2D _icon;
        [SerializeField] private string _individualInformation;

        public Texture2D GetIcon() => _icon;

        int IItem.GetId() => _id;

        string IItemIndividualInformation.GetInfo() => _individualInformation;

        string IItem.GetName() => _name;
    }
}
