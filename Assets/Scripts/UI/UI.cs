using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening; // Import DOTween namespace

public class UI : MonoBehaviour
{
    [SerializeField] private Image _creditsCanvas;
    [SerializeField] private Image _fadeImage; // UI image for fade transition (full-screen)

    private const string MainSceneName = "Main";

    private void Start()
    {
        if (_fadeImage != null)
        {
            _fadeImage.color = new Color(0, 0, 0, 0);
            _fadeImage.gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        if (Application.CanStreamedLevelBeLoaded(MainSceneName))
            StartSceneTransition(MainSceneName);
        else
            Debug.LogError($"Scene '{MainSceneName}' is not in the build settings!");
    }

    public void Credits()
    {
        if (_creditsCanvas == null)
        {
            Debug.LogError("Credits canvas is not assigned!");
            return;
        }
        _creditsCanvas.gameObject.SetActive(!_creditsCanvas.gameObject.activeSelf);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void StartSceneTransition(string sceneName)
    {
        if (_fadeImage == null)
        {
            Debug.LogError("Fade image is not assigned for scene transitions!");
            SceneManager.LoadScene(sceneName);
            return;
        }

        // Activate fade image and fade it in (black screen)
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(1f, 1f).OnComplete(() =>
        {
            // Load the new scene once the fade-in is complete
            SceneManager.LoadScene(sceneName);
        });
    }
}