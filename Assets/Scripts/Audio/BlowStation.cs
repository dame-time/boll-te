using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(BoxCollider))]
    public class BlowStation : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            Debug.Log("Player entered the blow station!");
        }
    }
}
