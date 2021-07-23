using GameInventory.UI;
using UnityEngine;

namespace GameInventory.Engine
{
    public class ObjectItemBehaviour : MonoBehaviour
    {
        [SerializeField] private BaseItem _item;

        private void Awake()
        {
            _item = Instantiate(_item);
        }

        private void OnMouseDown()
        {
            ObjectLocator.Resolve<Inventory>().Insert(0, 0, _item);
        }
    }
}
