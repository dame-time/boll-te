using UnityEngine;

namespace Clients.Orders
{
    public class Bubble : ItemBase
    {
        public BubbleType bubbleType;
        public string bubbleName;
        
        public override string GetItemString()
        {
            return bubbleName;
        }
    }
}
