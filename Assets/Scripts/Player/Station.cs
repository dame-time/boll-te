using System;
using System.Collections;
using Clients.Orders;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public enum StationType
    {
        None,
        Grabbable,
        Droppable,
        Both
    }
    
    public enum StationBothType
    {
        Grabbable,
        Droppable
    }
    
    public class Station : MonoBehaviour
    {
        
        public Item stationItem;
        
        public StationType stationType;
        public StationBothType initialStationStatus; 
        
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

        public void ExecuteAction()
        {
            if (stationType == StationType.Grabbable) GrabItem();
            else if (stationType == StationType.Droppable) DropItem();
            else if (stationType == StationType.Both)
            {
                switch (initialStationStatus)
                {
                    case StationBothType.Grabbable:
                        GrabItem();
                        initialStationStatus = StationBothType.Droppable;
                        break;
                    case StationBothType.Droppable:
                        DropItem();
                        initialStationStatus = StationBothType.Grabbable;
                        break;
                }
            }
        }
        
        public void GrabItem()
        {
            if (stationItem == null) return;
            if (!stationItem.gameObject.activeSelf || stationType != StationType.Grabbable) return;
            
            _playerBackpack.objectHolded = stationItem.gameObject;
            _playerBackpack.isHoldingObject = true;
            StartCoroutine(ReEnableItem());
        }

        private void DropItem()
        {
            if (stationItem == null) return;
            if (!stationItem.gameObject.activeSelf || stationType != StationType.Droppable) return;

            if (stationItem.itemType == ItemType.Cup)
            {
                if (!_playerBackpack.objectHolded) return;
                
                var bubble = _playerBackpack.objectHolded.GetComponent<Bubble>();
                
                if (bubble == null) return;
                
                var cup = stationItem.GetItem<Cup>();
                cup.bubble = _playerBackpack.objectHolded.GetComponent<Bubble>();
                
                _playerBackpack.objectHolded = null;
                _playerBackpack.isHoldingObject = false;
            }
            
            if (stationItem.itemType == ItemType.Bin)
            {
                _playerBackpack.objectHolded = null;
                _playerBackpack.isHoldingObject = false;
            }
            
            if (stationItem.itemType == ItemType.Tea)
            {
                if (!_playerBackpack.objectHolded) return;
                
                var cup = _playerBackpack.objectHolded.GetComponent<Cup>();
                
                if (cup == null) return;
                
                var tea = stationItem.GetItem<Tea>();
                
                cup.tea = tea;
                
                _playerBackpack.objectHolded = null;
                _playerBackpack.isHoldingObject = false;
            }
        }

        private IEnumerator ReEnableItem()
        {
            stationItem.gameObject.SetActive(false);
            yield return new WaitForSeconds(2.5f);
            stationItem.gameObject.SetActive(true);
        }
    }
}
