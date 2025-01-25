using UnityEngine;

namespace Clients.Orders
{
    public class Bubble : ItemBase
    {
        public string bubbleName;
        
        public override string GetItemString()
        {
            return bubbleName;
        }
    }
}
