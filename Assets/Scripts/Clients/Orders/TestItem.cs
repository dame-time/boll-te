using UnityEngine;

namespace Clients.Orders
{
    public class TestItem : MonoBehaviour
    {
        [SerializeField] private GameObject fruitPrefab;
        [SerializeField] private GameObject teaPrefab;

        void Start()
        {
            // Create an Item component
            var itemComponent = gameObject.AddComponent<Item>();

            // Instantiate and assign a Fruit item
            var fruitObject = Instantiate(fruitPrefab);
            var fruit = fruitObject.GetComponent<Fruit>();
            fruit.fruitName = "Apple";
            itemComponent.SetItem(fruit);

            Debug.Log(itemComponent.GetItemString()); // Output: "Fruit: Apple"

            // Retrieve the specific type
            var retrievedFruit = itemComponent.GetItem<Fruit>();
            if (retrievedFruit != null)
            {
                Debug.Log($"Retrieved fruit name: {retrievedFruit.fruitName}"); // Output: "Retrieved fruit name: Apple"
            }

            // Instantiate and assign a Tea item
            var teaObject = Instantiate(teaPrefab);
            var tea = teaObject.GetComponent<Tea>();
            tea.teaName = "Green Tea";
            itemComponent.SetItem(tea);

            Debug.Log(itemComponent.GetItemString()); // Output: "Tea: Green Tea"
        }
    }
}