using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Clients.Orders;
using Env;
using UnityEngine;
using UnityEngine.UI;

namespace Clients
{
    public class Client : MonoBehaviour
    {
        private Lane _lane;
        private ClientsPool _clientsPool;
        private int _currentLanePositionIndex = 1;
        private bool _isMoving = false;

        public Ticket _ticket;
        private List<OrderObject> _orders = new List<OrderObject>();
        private bool _terminated = false;
        
        private Collider _collider;

        public float timer;

        public Slider timeSlider;

        public int indexClient;

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

        public void setSlider(GameObject client)
        {
            timeSlider = client.GetComponentInChildren<Slider>();
            timeSlider.maxValue = timer;
            timeSlider.value = timer;
            timeSlider.gameObject.SetActive(false);
        }

        public void MoveTowardsNextPosition(int currentIndex)
        {
            //if (_isMoving) return;

            print("moving into lane " + _lane.lanePositions[_lane.lanePositions.Count - currentIndex]);
            StartCoroutine(MoveClient(_lane.laneStart.position, _lane.lanePositions[_lane.lanePositions.Count - currentIndex].position,true));

            //if (currentIndex == 1)
            //{
            //    StartCoroutine(MoveClient(_lane.laneStart.position, _lane.lanePositions[_lane.lanePositions.Count - currentIndex].position));
            //}
            //else if (currentIndex < _lane.lanePositions.Count - 1)
            //{
            //    StartCoroutine(MoveClient(
            //        _lane.laneStart.position,
            //        _lane.lanePositions[_lane.lanePositions.Count - currentIndex].position
            //    ));
            //}
            //else if (_currentLanePositionIndex == _lane.lanePositions.Count - 1)
            //{
            //    StartCoroutine(MoveClient(
            //        _lane.lanePositions[_currentLanePositionIndex].position,
            //        _lane.laneEnd.position
            //    ));
            //}
        }

        public void MoveListPosition()
        {
            print("moving into lane " + _lane.lanePositions[indexClient]);
            StartCoroutine(MoveClient(this.transform.position, _lane.lanePositions[indexClient].position, false));
            indexClient++;
        }

        private void Update()
        {
            _collider.enabled = _clientsPool.PeekClient() == this;
            
            if (_ticket.isInProgress || _terminated) return;
            
            _terminated = true;
            StopAllCoroutines();
            StartCoroutine(MoveClient(
                _lane.lanePositions[_currentLanePositionIndex].position,
                _lane.laneEnd.position, false
            ));
        }

        private IEnumerator MoveClient(Vector3 start, Vector3 end, bool firstEntry)
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

            if (transform.position == _lane.laneEnd.position)
            {
                print("POPPAAAAA");
                _clientsPool.PopClient(this.gameObject);
            }

            if (firstEntry)
            {
                _isMoving = false;
                timeSlider.gameObject.SetActive(true);
                StartCoroutine(TimerDecrease(timer));
            }

        }

        private IEnumerator TimerDecrease(float timer)
        {
            while (timer > 0)
            {
                //print($"current timer: {timer}");
                // Decrease the timer
                timer -= Time.deltaTime;

                // Update the slider
                timeSlider.value = timer;

                // Wait for the next frame
                yield return null;
            }

            // Ensure the timer stops exactly at 0
            timer = 0;
            timeSlider.value = 0;
            timeSlider.gameObject.SetActive(false);
            StartCoroutine(MoveClient(this.transform.position, _lane.laneEnd.position, false));
            _clientsPool._clients.Remove(this.gameObject);
            print("current index of client = " + _clientsPool.currentIndex);
            int tempIndex = _clientsPool.currentIndex-1;
            foreach (GameObject currentClient in _clientsPool._clients)
            {

                currentClient.GetComponent<Client>().MoveListPosition();

            }
            _clientsPool.currentIndex--;
            //_clientsPool.PopClient();
            Debug.Log("Timer finished!");
        }
    }
}