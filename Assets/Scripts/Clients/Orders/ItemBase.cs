using UnityEngine;
using UnityEngine.Serialization;

namespace Clients.Orders
{
    public enum ItemType
    {
        None,
        Fruit,
        Tea,
        Bubble,
        Cup
    }
    
    public abstract class ItemBase : MonoBehaviour
    {
        public int id;
        
        public abstract string GetItemString();
    }
}
