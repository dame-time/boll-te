using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Clients;
using Clients.Orders;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Image recipesImage;

    public Image GameOverImage;
    public TextMeshProUGUI finalScore;

    public SerializedDictionary<string, GameObject> fruitMapper = new SerializedDictionary<string, GameObject>();
    public SerializedDictionary<GameObject, BubbleType> bubbleMapper = new SerializedDictionary<GameObject, BubbleType>();

    private float timer = 5;

    public int money = 0;

    public AudioSource managerAudio;
    public AudioClip clientArrive;
    public AudioClip openRecipes;

    private ScoreAnimator _scoreAnimator;

    [System.Serializable] // Questo permette di vedere la struct nell'Inspector
    public struct TheData
    {
        public TeaType teaType;
        public BubbleType bubbleType;
        public Sprite image;
        public int value;   
    }

    public TheData[] TheArrayType;

    [SerializeField] private int clientsToSpawn = 5;
    
    private ClientsPool _clientsPool;
    
    private bool isRecipeVisible = false;
    private float fadeDuration = 0.2f;
    
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        _clientsPool = FindObjectOfType<ClientsPool>();

        managerAudio.Play();
        managerAudio.loop = true;

        _scoreAnimator = FindObjectOfType<ScoreAnimator>();
    }
    
    private void PlayGameOver()
    {

        if(_clientsPool._clients.Count > 0)
        {
            StartCoroutine(CheckGameOver());
        }
        else
        {
            Debug.Log("Game Over");
            GameOverImage.gameObject.SetActive(true);
            Time.timeScale = 0f;
            finalScore.text = _scoreAnimator.currentScore.ToString();
        }

    }

    IEnumerator CheckGameOver()
    {
        yield return new WaitForSeconds(10f);
        PlayGameOver();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleRecipesImage();
            managerAudio.PlayOneShot(openRecipes);
        }
        if (clientsToSpawn == 0)
        {
            PlayGameOver();
            return;
        }
        
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = Random.Range(20,30);
            _clientsPool.AddClient();
            managerAudio.PlayOneShot(clientArrive);
            --clientsToSpawn;
        }
        
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    _clientsPool.AddClient();
        //    managerAudio.PlayOneShot(clientArrive);
        //    // _clientsPool.MoveClients();
        //}



        
    }
    
    private void ToggleRecipesImage()
    {
        if (recipesImage == null)
        {
            Debug.LogError("Recipes Image is not assigned!");
            return;
        }

        recipesImage.DOKill();

        if (isRecipeVisible)
        {
            recipesImage.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                recipesImage.gameObject.SetActive(false);
            });
        }
        else
        {
            recipesImage.gameObject.SetActive(true);
            recipesImage.DOFade(1f, fadeDuration);
        }

        isRecipeVisible = !isRecipeVisible;
    }
}
