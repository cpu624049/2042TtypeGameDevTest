using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeInputHandler : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float minSwipeDistance = 50f;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isDragging;

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI())
                return;

            isDragging = true;
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endPosition = Input.mousePosition;
            isDragging = false;

            SwipeDirection direction = GetSwipeDirection(startPosition, endPosition);
            gameManager.TryHandleSwipe(direction);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (IsPointerOverUI())
                    return;

                isDragging = true;
                startPosition = touch.position;
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (!isDragging)
                    return;

                endPosition = touch.position;
                isDragging = false;

                SwipeDirection direction = GetSwipeDirection(startPosition, endPosition);
                gameManager.TryHandleSwipe(direction);
                break;
        }
    }

    private SwipeDirection GetSwipeDirection(Vector2 start, Vector2 end)
    {
        Vector2 delta = end - start;

        if (delta.magnitude < minSwipeDistance)
            return SwipeDirection.None;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            return delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}