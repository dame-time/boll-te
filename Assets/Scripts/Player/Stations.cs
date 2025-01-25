using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class Stations : MonoBehaviour
    {
        private Queue<Station> _stationQueue;
        
        public void Start()
        {
            _stationQueue = new Queue<Station>();
        }
        
        public void AddStation(Station station)
        {
            if (_stationQueue.Contains(station)) return;
            
            _stationQueue.Enqueue(station);
        }
        
        public Station GetActiveStation()
        {
            return _stationQueue.Peek();
        }

        public void RemoveStation(Station station)
        {
            if (_stationQueue.Peek() == station)
            {
                _stationQueue.Dequeue();
                return;
            }

            var tmp = _stationQueue;
            _stationQueue = new Queue<Station>();
            foreach (var s in tmp.Where(s => s != station))
                _stationQueue.Enqueue(s);
        }

        private void Update()
        {
            if (_stationQueue.Count == 0) return;
            
            var activeStation = GetActiveStation();
            Debug.Log($"Active station: {activeStation.name}");
        }
    }
}
