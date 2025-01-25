namespace Clients.Orders
{
    public class Tea : ItemBase
    {
        public string teaName;
        
        public TeaType teaType;
        
        public override string GetItemString()
        {
            return teaName;
        }
    }
}
