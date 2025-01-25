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

    public enum ExecutedAction
    {
        None,
        Grabbed,
        Dropped
    }
    
    public class Station : MonoBehaviour
    {
        
        public Item stationItem;
        
        public StationType stationType;
        public StationBothType initialStationStatus; 
        
        private Stations _stations;
        private PlayerBackpack _playerBackpack;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _stations = FindObjectOfType<Stations>();
            if (stationItem == null) GetComponentInChildren<Item>();
            _playerBackpack = FindObjectOfType<PlayerBackpack>();
            _playerMovement = FindObjectOfType<PlayerMovement>();
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

        public ExecutedAction ExecuteAction()
        {
            if (stationType == StationType.Grabbable) return GrabItem();
            if (stationType == StationType.Droppable) return DropItem();
            if (stationType == StationType.Both)
            {
                switch (initialStationStatus)
                {
                    case StationBothType.Grabbable:
                        var grabAction = GrabItem();
                        if (grabAction == ExecutedAction.None) return ExecutedAction.None;
                        initialStationStatus = StationBothType.Droppable;
                        return grabAction;
                        break;
                    case StationBothType.Droppable:
                        var dropAction = DropItem();
                        if (dropAction == ExecutedAction.None) return ExecutedAction.None;
                        initialStationStatus = StationBothType.Grabbable;
                        return dropAction;
                        break;
                }
            }
            
            return ExecutedAction.None;
        }
        
        public ExecutedAction GrabItem()
        {
            if (stationItem == null) return ExecutedAction.None;
            if (!stationItem.gameObject.activeSelf || stationType != StationType.Grabbable) return ExecutedAction.None;
            
            _playerBackpack.objectHolded = stationItem.gameObject;
            _playerBackpack.isHoldingObject = true;
            StartCoroutine(ReEnableItem());
            
            return ExecutedAction.Grabbed;
        }

        private ExecutedAction DropItem()
        {
            if (stationItem == null) return ExecutedAction.None;
            if (!stationItem.gameObject.activeSelf || stationType != StationType.Droppable) return ExecutedAction.None;

            if (stationItem.itemType == ItemType.Cup)
            {
                if (!_playerBackpack.objectHolded) return ExecutedAction.None;
                
                var bubble = _playerBackpack.objectHolded.GetComponent<Bubble>();
                
                if (bubble == null) return ExecutedAction.None;
                
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
                if (!_playerBackpack.objectHolded) return ExecutedAction.None;
                
                var cup = _playerBackpack.objectHolded.GetComponent<Cup>();
                
                if (cup == null) return ExecutedAction.None;
                
                var tea = stationItem.GetItem<Tea>();
                
                cup.tea = tea;
                
                _playerBackpack.objectHolded = null;
                _playerBackpack.isHoldingObject = false;
            }

            _playerMovement.Drop();
            return ExecutedAction.Dropped;
        }

        private IEnumerator ReEnableItem()
        {
            stationItem.gameObject.SetActive(false);
            yield return new WaitForSeconds(2.5f);
            stationItem.gameObject.SetActive(true);
        }
    }
}
