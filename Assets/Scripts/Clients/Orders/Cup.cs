using System;

namespace Clients.Orders
{
    public class Cup : ItemBase
    {
        public string cupName;
        
        public Bubble bubble;
        public Tea tea;

        private void Awake()
        {
            tea = null;
            bubble = null;
        }

        public void Reset()
        {
            tea = null;
            bubble = null;
        }
        
        public override string GetItemString()
        {
            return cupName;
        }
    }
}
