using System;
using System.Collections;
using Clients.Orders;
using UnityEngine;

namespace Player
{
    public class Station : MonoBehaviour
    {
        public Item stationItem;
        
        private Stations _stations;
        private PlayerBackpack _playerBackpack;

        private void Awake()
        {
            _stations = FindObjectOfType<Stations>();
            if (stationItem == null) GetComponentInChildren<Item>();
            _playerBackpack = FindObjectOfType<PlayerBackpack>();
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
        
        public void GrabItem()
        {
            if (stationItem == null || !stationItem.gameObject.activeSelf) return;
            
            _playerBackpack.objectHolded = stationItem.gameObject;
            _playerBackpack.isHoldingObject = true;
            StartCoroutine(ReEnableItem());
        }
        
        IEnumerator ReEnableItem()
        {
            stationItem.gameObject.SetActive(false);
            yield return new WaitForSeconds(2.5f);
            stationItem.gameObject.SetActive(true);
        }
    }
}
