using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreAnimator : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro text
    public float animationDuration = 0.5f; // Duration of the animation
    public float scaleMultiplier = 1.5f; // Scale effect during animation
    public Color gainColor = Color.yellow; // Temporary color for the animation

    public int currentScore = 0; // Tracks the current score
    private Color originalColor; // Original color of the text

    private void Start()
    {
        // Store the original color of the text
        if (scoreText != null)
        {
            originalColor = scoreText.color;
            UpdateScoreDisplay();
        }
        else
        {
            Debug.LogError("Score Text is not assigned!");
        }
    }

    public void AddScore(int points)
    {
        if (scoreText == null) return;

        currentScore += points;

        Vector3 originalPosition = scoreText.transform.localPosition;

        Sequence animationSequence = DOTween.Sequence();

        animationSequence.Append(scoreText.transform
                .DOLocalMoveY(originalPosition.y + 20f, animationDuration * 0.5f))
            .Append(scoreText.transform.DOLocalMoveY(originalPosition.y, animationDuration * 0.5f));

        animationSequence.Join(scoreText.DOFade(0.5f, animationDuration * 0.5f))
            .Append(scoreText.DOFade(1f, animationDuration * 0.5f));

        animationSequence.Append(scoreText.DOColor(gainColor, animationDuration * 0.5f))
            .Append(scoreText.DOColor(originalColor, animationDuration * 0.5f));

        animationSequence.Join(scoreText.transform.DOScale(scaleMultiplier, animationDuration * 0.5f))
            .Append(scoreText.transform.DOScale(1f, animationDuration * 0.5f));

        animationSequence.OnStart(UpdateScoreDisplay);

        animationSequence.Play();
    }


    private void UpdateScoreDisplay()
    {
        scoreText.text = currentScore.ToString();
    }
}