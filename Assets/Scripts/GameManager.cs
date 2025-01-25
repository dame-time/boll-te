using Clients;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int clientsToSpawn = 5;
    
    private ClientsPool _clientsPool;
    
    private void Start()
    {
        _clientsPool = FindObjectOfType<ClientsPool>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _clientsPool.AddClient();
            _clientsPool.MoveClients();
        }
    }
}
