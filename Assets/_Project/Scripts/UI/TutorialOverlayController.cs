using TMPro;
using UnityEngine;

public class TutorialOverlayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI guideText;

    private const string TutorialShownKey = "TUTORIAL_SHOWN";
    private bool isVisible = false;

    private void Awake()
    {
        HideImmediate();
    }

    private void Update()
    {
        if (!isVisible)
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            CompleteTutorial();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                CompleteTutorial();
            }
        }
#endif
    }

    public void TryShowFirstTime()
    {
        bool alreadyShown = PlayerPrefs.GetInt(TutorialShownKey, 0) == 1;

        if (alreadyShown)
        {
            HideImmediate();
            return;
        }

        Show();
    }

    public void Show()
    {
        isVisible = true;
        gameObject.SetActive(true);

        if (guideText != null)
        {
            guideText.text = "스와이프해서 같은 숫자를 합치세요";
        }

        Debug.Log("TutorialOverlay.Show()");
    }

    public void HideImmediate()
    {
        isVisible = false;
        gameObject.SetActive(false);
    }

    private void CompleteTutorial()
    {
        PlayerPrefs.SetInt(TutorialShownKey, 1);
        PlayerPrefs.Save();

        HideImmediate();
        Debug.Log("TutorialOverlay.Completed");
    }

    public bool IsVisible()
    {
        return isVisible;
    }

    public void ResetTutorialForDebug()
    {
        PlayerPrefs.DeleteKey(TutorialShownKey);
        PlayerPrefs.Save();
        Debug.Log("TutorialOverlay.ResetTutorialForDebug()");
    }
}