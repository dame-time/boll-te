using UnityEngine;
using UnityEngine.Serialization;

namespace Clients.Orders
{
    public class Fruit : ItemBase
    {
        public BubbleType bubbleType;
        public string fruitName;
        
        public override string GetItemString()
        {
            return fruitName;
        }
    }
}
