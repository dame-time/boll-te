using Clients.Orders;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OrderObject))]
public class OrderObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var order = (OrderObject)target;
        
        if (!GUILayout.Button("Reset Order")) return;
        
        order.orderName = "Default Order";
        order.orderTime = 0;
        order.orderValue = 0;
        EditorUtility.SetDirty(order);
    }
}