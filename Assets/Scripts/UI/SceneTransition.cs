using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;

    private void Start()
    {
        if (_fadeImage != null)
        {
            _fadeImage.color = Color.black;
            _fadeImage.DOFade(0f, 1f).OnComplete(() =>
            {
                _fadeImage.gameObject.SetActive(false);
            });
        }
    }
}