using System;
using UnityEngine;

namespace Clients.Orders
{
    public enum CupStatus
    {
        Empty,
        Bubbles,
        Full
    }

    public class Cup : ItemBase
    {
        public string cupName;

        public CupStatus cupStatus;
        public GameObject cupEmpty;
        public GameObject cupBubbles;
        public GameObject cupFull;
        
        public BubbleType bubble;
        public TeaSize teaSize;
        public TeaType tea;
        
        private MeshFilter _meshFilter;

        private void Awake()
        {
            tea = TeaType.None;
            bubble = BubbleType.None;
            
            _meshFilter = GetComponent<MeshFilter>();
        }

        public void SetCupNextStatus()
        {
            switch (cupStatus)
            {
                case CupStatus.Empty:
                    cupStatus = CupStatus.Bubbles;
                    UpdateMesh(cupBubbles.GetComponent<MeshFilter>().sharedMesh);
                    break;
                case CupStatus.Bubbles:
                    cupStatus = CupStatus.Full;
                    UpdateMesh(cupFull.GetComponent<MeshFilter>().sharedMesh);
                    break;
                case CupStatus.Full:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateMesh(Mesh newMesh)
        {
            if (newMesh == null)
            {
                Debug.LogWarning("New mesh is null.");
                return;
            }

            var currentBounds = _meshFilter.sharedMesh.bounds;

            _meshFilter.mesh = newMesh;

            var newBounds = newMesh.bounds;

            Vector3 offset = currentBounds.center - newBounds.center;

            transform.localPosition += offset;
        }

        public void ResetCup()
        {
            cupStatus = CupStatus.Empty;
            _meshFilter.mesh = cupEmpty.GetComponent<MeshFilter>().sharedMesh;
        }
        
        public override string GetItemString()
        {
            return cupName;
        }
    }
}
