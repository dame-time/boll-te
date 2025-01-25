using UnityEngine;

namespace Clients.Orders
{
    public class Item : MonoBehaviour
    {
        public ItemType itemType;
        public ItemBase item;

        public void SetItem<T>(T newItem) where T : ItemBase
        {
            item = newItem;
            itemType = GetItemTypeFromInstance(newItem);
        }

        public T GetItem<T>() where T : ItemBase
        {
            switch (itemType)
            {
                case ItemType.Fruit when typeof(T) == typeof(Fruit):
                    return item as T;
                case ItemType.Tea when typeof(T) == typeof(Tea):
                    return item as T;
                case ItemType.Bubble when typeof(T) == typeof(Bubble):
                    return item as T;
                case ItemType.Cup when typeof(T) == typeof(Cup):
                    return item as T;
                case ItemType.Bubbler when typeof(T) == typeof(Bubbler):
                    return item as T;
                case ItemType.None:
                default:
                    Debug.LogWarning($"GetItem<{typeof(T).Name}> failed: itemType is {itemType}");
                    return null;
            }
        }

        public string GetItemString()
        {
            return item?.GetItemString() ?? "No item";
        }

        private ItemType GetItemTypeFromInstance(ItemBase instance)
        {
            return instance switch
            {
                Fruit => ItemType.Fruit,
                Tea => ItemType.Tea,
                Bubble => ItemType.Bubble,
                Cup => ItemType.Cup,
                _ => ItemType.None
            };
        }
    }
}