using Clients.Orders;
using UnityEngine;

namespace Player
{
    public class PlayerBackpack : MonoBehaviour
    {
        public GameObject objectHolded;
        public BubbleType bubbleType;
        public GameObject cupFull;
        public TeaSize teaSize;
        public TeaType teaType;
        public bool isHoldingObject;

        public void Clear()
        {
            objectHolded = null;
            bubbleType = BubbleType.None;
            cupFull = null;
            teaSize = TeaSize.None;
            teaType = TeaType.None;
            isHoldingObject = false;
        }
    }
}
