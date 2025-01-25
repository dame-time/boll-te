namespace Clients.Orders
{
    public class Tea : ItemBase
    {
        public string teaName;
        
        public override string GetItemString()
        {
            return teaName;
        }
    }
}
