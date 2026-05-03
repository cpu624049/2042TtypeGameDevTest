using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private HUDController hudController;
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private TutorialOverlayController tutorialOverlayController;
    [SerializeField] private AudioManager audioManager;

    private GameState currentState = GameState.Ready;
    private int currentScore = 0;
    private int bestScore = 0;
    private bool continueUsed = false;

    private const string BestScoreKey = "BEST_SCORE";

    private void Start()
    {
        LoadBestScore();
        StartGame();
    }

    public void StartGame()
    {
        currentScore = 0;
        currentState = GameState.Playing;
        continueUsed = false;

        if (boardManager == null)
        {
            Debug.LogError("GameManager: boardManager 참조가 비어 있음");
            return;
        }

        boardManager.InitializeBoard();

        if (!boardManager.IsUsingDebugBoard())
        {
            boardManager.SpawnStartTiles();
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.Hide();
        }

        if (tutorialOverlayController != null)
        {
            tutorialOverlayController.TryShowFirstTime();
        }

        RefreshScoreUI();
        Debug.Log("GameManager.StartGame()");
    }

    public void TryHandleSwipe(SwipeDirection direction)
    {
        if (currentState != GameState.Playing)
            return;

        if (direction == SwipeDirection.None)
            return;

        Debug.Log($"GameManager.TryHandleSwipe() direction = {direction}");

        bool moved = boardManager.HandleMove(direction);

        if (!moved)
        {
            Debug.Log("이동 실패");

            if (boardManager.IsGameOver())
            {
                ShowGameOver();
            }

            return;
        }

        if (audioManager != null)
        {
            audioManager.PlayMove();
        }

        int gainedScore = boardManager.ConsumeLastMoveScore();

        if (gainedScore > 0 && audioManager != null)
        {
            audioManager.PlayMerge();
        }

        AddScore(gainedScore);

        boardManager.SpawnRandomTile();

        if (boardManager.IsGameOver())
        {
            ShowGameOver();
        }
    }

    private void AddScore(int amount)
    {
        if (amount <= 0)
            return;

        currentScore += amount;

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.Save();
        }

        RefreshScoreUI();
        Debug.Log($"점수 증가: +{amount}, 현재 점수: {currentScore}, 최고 점수: {bestScore}");
    }

    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    private void RefreshScoreUI()
    {
        if (hudController != null)
        {
            hudController.RefreshScore(currentScore, bestScore);
        }

        Debug.Log($"현재 점수: {currentScore}, 최고 점수: {bestScore}");
    }

    public void TryContinue()
    {
        if (currentState != GameState.GameOver)
            return;

        if (continueUsed)
            return;

        bool success = boardManager.RemoveLowestTileForContinue();

        if (!success)
        {
            Debug.Log("이어하기 실패");
            return;
        }

        continueUsed = true;
        currentState = GameState.Playing;

        if (gameOverPopup != null)
        {
            gameOverPopup.Hide();
        }

        Debug.Log("이어하기 성공");
        
        if (boardManager.IsGameOver())
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        currentState = GameState.GameOver;
        Debug.Log("Game Over");

        if (gameOverPopup != null)
        {
            gameOverPopup.Show(currentScore, !continueUsed);
        }
    }
}