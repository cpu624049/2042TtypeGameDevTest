using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnClickRestart);
        }
    }

    public void RefreshScore(int currentScore, int bestScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }

        if (bestScoreText != null)
        {
            bestScoreText.text = $"Best: {bestScore}";
        }
    }

    private void OnClickRestart()
    {
        if (gameManager != null)
        {
            gameManager.StartGame();
        }
    }
}