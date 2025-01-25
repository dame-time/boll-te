using System;
using System.Collections;
using System.Collections.Generic;
using Clients;
using Clients.Orders;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    public List<OrderObject> possibleOrders;
    public int maxOrders = 5;
    
    public Queue<Ticket> tickets = new Queue<Ticket>();
    
    private ClientsPool _clientsPool;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        _clientsPool = FindObjectOfType<ClientsPool>();
    }

    public List<OrderObject> PickRandomOrders()
    {
        var orders = new List<OrderObject>();
        
        var rng = Random.Range(1, maxOrders + 1);
        for (var i = 0; i < rng; i++) 
            orders.Add(possibleOrders[Random.Range(0, possibleOrders.Count)]);

        return orders;
    }

    private void Update()
    {
        if (tickets.Count == 0) return;
        
        var ticket = tickets.Peek();
        if (ticket.isComplete)
        {
            // Give the player money
            tickets.Dequeue();
            return;
        }

        if (!ticket.isFailed) return;
        
        tickets.Dequeue();
    }
}
