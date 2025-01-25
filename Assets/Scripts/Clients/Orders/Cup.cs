namespace Clients.Orders
{
    public class Cup : ItemBase
    {
        public string cupName;
        
        public override string GetItemString()
        {
            return cupName;
        }
    }
}
