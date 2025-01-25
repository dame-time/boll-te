using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;

namespace Clients.Orders
{
    public class Ticket
    {
        public string TheName;
        public Image theIcon;


        //public Client referenceClient;
        //public List<OrderObject> order;
        //public int totalValue;
        
        //public bool isComplete;
        //public bool isInProgress;
        //public bool isFailed;
        
        //public Ticket(Client client, List<OrderObject> orders)
        //{
        //    referenceClient = client;
        //    order = orders;
        //    totalValue = 0;
        //    foreach (var _order in orders)
        //        totalValue += _order.orderValue;
            
        //    isComplete = false;
        //    isInProgress = true;
        //    isFailed = false;
        //}
        
        //public void ProcessOrder(OrderObject _order)
        //{
        //    if (isComplete || isFailed) return;
            
        //    if (this.order.Contains(_order))
        //        this.order.Remove(_order);
        //    else
        //    {
        //        isFailed = true;
        //        isInProgress = false;
        //    }

        //    if (this.order.Count != 0) return;
            
        //    isComplete = true;
        //    isInProgress = false;
        //}
    }
}
