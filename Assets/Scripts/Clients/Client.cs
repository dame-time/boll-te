using System.Collections;
using System.Collections.Generic;
using Clients.Orders;
using Env;
using UnityEngine;

namespace Clients
{
    public class Client : MonoBehaviour
    {
        private Lane _lane;
        private ClientsPool _clientsPool;
        private int _currentLanePositionIndex = -1;
        private bool _isMoving = false;

        public Ticket _ticket;
        private List<OrderObject> _orders = new List<OrderObject>();
        private bool _terminated = false;
        
        private Collider _collider;

        public void Initialize()
        {
            _lane = FindObjectOfType<Lane>();
            _clientsPool = FindObjectOfType<ClientsPool>();
            LevelManager.Instance.PickRandomOrders().ForEach(order => _orders.Add(order));
            _ticket = new Ticket(this, _orders);
            LevelManager.Instance.tickets.Enqueue(_ticket);
            
            _collider = GetComponentInChildren<Collider>();
            _collider.enabled = false;

            foreach (var order in _orders)
                Debug.Log(order.orderName);
            Debug.Log("---------");
        }

        public void MoveTowardsNextPosition()
        {
            if (_isMoving) return;

            if (_currentLanePositionIndex == -1)
            {
                StartCoroutine(MoveClient(_lane.laneStart.position, _lane.lanePositions[_lane.lanePositions.Count -1].position));
                _currentLanePositionIndex = 0;
            }
            else if (_currentLanePositionIndex < _lane.lanePositions.Count - 1)
            {
                StartCoroutine(MoveClient(
                    _lane.lanePositions[_currentLanePositionIndex].position,
                    _lane.lanePositions[++_currentLanePositionIndex].position
                ));
            }
            else if (_currentLanePositionIndex == _lane.lanePositions.Count - 1)
            {
                StartCoroutine(MoveClient(
                    _lane.lanePositions[_currentLanePositionIndex].position,
                    _lane.laneEnd.position
                ));
            }
        }

        private void Update()
        {
            _collider.enabled = _clientsPool.PeekClient() == this;
            
            if (_ticket.isInProgress || _terminated) return;
            
            _terminated = true;
            StopAllCoroutines();
            StartCoroutine(MoveClient(
                _lane.lanePositions[_currentLanePositionIndex].position,
                _lane.laneEnd.position
            ));
        }

        private IEnumerator MoveClient(Vector3 start, Vector3 end)
        {
            _isMoving = true;

            var duration = Mathf.Sqrt((start - end).sqrMagnitude) / 3.0f;
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(start, end, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = end;

            if (transform.position == _lane.laneEnd.position) _clientsPool.PopClient();

            _isMoving = false;
        }
    }
}