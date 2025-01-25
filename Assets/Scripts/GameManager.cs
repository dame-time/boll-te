using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Clients;
using Clients.Orders;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SerializedDictionary<string, GameObject> fruitMapper = new SerializedDictionary<string, GameObject>();
    public SerializedDictionary<GameObject, BubbleType> bubbleMapper = new SerializedDictionary<GameObject, BubbleType>();
    
    [SerializeField] private int clientsToSpawn = 5;
    
    private ClientsPool _clientsPool;
    
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        _clientsPool = FindObjectOfType<ClientsPool>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _clientsPool.AddClient();
            // _clientsPool.MoveClients();
        }
    }
}
