using UnityEngine;

namespace Clients.Orders
{
    [CreateAssetMenu(fileName = "NewOrder", menuName = "Clients/Order", order = 1)]
    public class OrderObject : ScriptableObject
    {
        public string orderName;
        public float orderTime;
        
        public TeaType teaType;
        public TeaSize teaSize;
        
        public BubbleType bubbleType;
        
        public int orderValue;
    }
}
