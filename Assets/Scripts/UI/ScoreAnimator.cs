using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreAnimator : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro text
    public float animationDuration = 0.5f; // Duration of the animation
    public float scaleMultiplier = 1.5f; // Scale effect during animation
    public Color gainColor = Color.yellow; // Temporary color for the animation

    private int currentScore = 0; // Tracks the current score
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

        // Animate the text
        Sequence animationSequence = DOTween.Sequence();

        // Change color to gainColor, then back to original
        animationSequence.Append(scoreText.DOColor(gainColor, animationDuration * 0.5f))
            .Append(scoreText.DOColor(originalColor, animationDuration * 0.5f));

        // Scale up and back to normal
        animationSequence.Join(scoreText.transform.DOScale(scaleMultiplier, animationDuration * 0.5f))
            .Append(scoreText.transform.DOScale(1f, animationDuration * 0.5f));

        // Update the text at the start of the animation
        animationSequence.OnStart(UpdateScoreDisplay);

        // Play the sequence
        animationSequence.Play();
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {currentScore}";
    }
}