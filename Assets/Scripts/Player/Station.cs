using System;
using UnityEngine;

namespace Player
{
    public class Station : MonoBehaviour
    {
        public GameObject stationObject;
        
        private Stations _stations;

        private void Awake()
        {
            _stations = FindObjectOfType<Stations>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (_stations == null) return;
            
            _stations.AddStation(this);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (_stations == null) return;
            
            _stations.RemoveStation(this);
        }
    }
}
