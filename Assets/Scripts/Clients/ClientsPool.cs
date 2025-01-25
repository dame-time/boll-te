using System.Collections.Generic;
using System.Linq;
using Env;
using UnityEngine;

namespace Clients
{
    public class ClientsPool : MonoBehaviour
    {
        [SerializeField] private GameObject clientPrefab;

        private List<GameObject> _clients = new List<GameObject>();
        private Lane _lane;

        private void Start()
        {
            _lane = FindObjectOfType<Lane>();
        }

        public void MoveClients()
        {
            foreach (var client in _clients.Select(t => t.GetComponent<Client>())) client.MoveTowardsNextPosition();
        }

        public void AddClient()
        {
            var client = Instantiate(clientPrefab, transform);
            var clientComponent = client.AddComponent<Client>();
            clientComponent.Initialize();
            client.transform.position = _lane.laneStart.position;
            _clients.Add(client);
        }

        public void PopClient()
        {
            if (_clients.Count == 0) return;
            
            var client = _clients[0];
            _clients.RemoveAt(0);
            Destroy(client);
        }
    }
}
