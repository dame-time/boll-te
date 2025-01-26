using System;
using System.Collections.Generic;
using System.Linq;
using Env;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Clients
{
    public class ClientsPool : MonoBehaviour
    {
        [SerializeField] private GameObject clientPrefab;

        public List<GameObject> _clients = new List<GameObject>();
        private Lane _lane;

        public int currentIndex = 0;



        private void Start()
        {
            _lane = FindObjectOfType<Lane>();
        }

        //public void MoveClients()
        //{
        //    foreach (var client in _clients.Select(t => t.GetComponent<Client>())) client.MoveTowardsNextPosition(currentIndex);
        //}

        public void AddClient()
        {
            var client = Instantiate(clientPrefab, transform);
            var clientComponent = client.AddComponent<Client>();
            clientComponent.Initialize();
            clientComponent.timer = Random.Range(45, 60);
            //clientComponent.timeSlider.maxValue = clientComponent.timer;
            //clientComponent.timeSlider.value = clientComponent.timer;
            client.transform.position = _lane.laneStart.position;
            _clients.Add(client);
            clientComponent.MoveTowardsNextPosition(currentIndex+1);
            clientComponent.setSlider(client);
            //print("current index before client = " + currentIndex);
            currentIndex++;
            clientComponent.indexClient = _lane.lanePositions.Count - currentIndex;
            //print("current index of client = " + currentIndex);


        }
        
        public Client PeekClient()
        {
            return _clients.Count == 0 ? null : _clients[0].GetComponent<Client>();
        }

        public void PopClient(GameObject clientToDestroy)
        {
            //if (_clients.Count == 0) return;
            
            //var client = _clients[0];
            //_clients.RemoveAt(0);
            Destroy(clientToDestroy);

            //foreach (GameObject currentClient in _clients)
            //{
            //    if (currentClient != null)
            //    {
            //        currentClient.GetComponent<Client>().MoveListPosition(currentIndex);
            //        currentIndex--;
            //    }
            //}

        }
    }
}
