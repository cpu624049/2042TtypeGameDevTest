using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(OnClickContinue);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnClickRestart);
    }

    public void Show(int currentScore, bool canContinue)
    {
        Debug.Log($"GameOverPopup.Show() / score={currentScore} / canContinue={canContinue}");
        gameObject.SetActive(true);

        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }

        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(canContinue);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickContinue()
    {
        if (gameManager != null)
        {
            gameManager.TryContinue();
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