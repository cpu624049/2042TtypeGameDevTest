using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;

    private GameState currentState = GameState.Ready;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        currentState = GameState.Playing;

        if (boardManager == null)
        {
            Debug.LogError("GameManager: boardManager 참조가 비어 있음");
            return;
        }

        boardManager.InitializeBoard();
        Debug.Log("GameManager.StartGame()");
    }

    public void TryHandleSwipe(SwipeDirection direction)
    {
        if (currentState != GameState.Playing)
            return;

        if (direction == SwipeDirection.None)
            return;

        Debug.Log($"GameManager.TryHandleSwipe() direction = {direction}");
        boardManager.HandleMove(direction);
    }
}