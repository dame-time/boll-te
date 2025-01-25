using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Clients.Orders
{
    public class Bubbler : ItemBase
    {
        public BubbleType bubbleType;

        private void Awake()
        {
            bubbleType = BubbleType.None;
        }

        public override string GetItemString()
        {
            return "Bubbler";
        }
    }
}
