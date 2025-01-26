using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Image _creditsCanvas;

    private const string MainSceneName = "Main";

    public void Play()
    {
        if (Application.CanStreamedLevelBeLoaded(MainSceneName))
        {
            SceneManager.LoadScene(MainSceneName);
        }
        else
        {
            Debug.LogError($"Scene '{MainSceneName}' is not in the build settings!");
        }
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
}