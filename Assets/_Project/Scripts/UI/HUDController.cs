using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private TextMeshProUGUI soundButtonText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnClickRestart);
        }

        if (soundButton != null)
        {
            soundButton.onClick.AddListener(OnClickSoundToggle);
        }

        RefreshSoundButtonUI();
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
        if (audioManager != null)
        {
            audioManager.PlayClick();
        }

        if (gameManager != null)
        {
            gameManager.StartGame();
        }
    }

    private void OnClickSoundToggle()
    {
        if (audioManager != null)
        {
            audioManager.PlayClick();
            audioManager.ToggleSound();
        }

        RefreshSoundButtonUI();
    }

    private void RefreshSoundButtonUI()
    {
        if (audioManager == null || soundButtonText == null)
            return;

        soundButtonText.text = audioManager.IsSoundOn() ? "Sound On" : "Sound Off";
    }
}