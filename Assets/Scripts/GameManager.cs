using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Clients;
using Clients.Orders;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Image recipesImage;

    public SerializedDictionary<string, GameObject> fruitMapper = new SerializedDictionary<string, GameObject>();
    public SerializedDictionary<GameObject, BubbleType> bubbleMapper = new SerializedDictionary<GameObject, BubbleType>();

    [System.Serializable] // Questo permette di vedere la struct nell'Inspector
    public struct TheData
    {
        public string name;      // Nome dell'oggetto
        public Sprite image;     // Immagine dell'oggetto
        public int value;        // Valore dell'oggetto
    }

    public TheData[] TheArrayType;

    [SerializeField] private int clientsToSpawn = 5;
    
    private ClientsPool _clientsPool;
    
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        _clientsPool = FindObjectOfType<ClientsPool>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _clientsPool.AddClient();
            // _clientsPool.MoveClients();
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
            recipesImage.gameObject.SetActive(!recipesImage.gameObject.activeSelf);
    }
}
