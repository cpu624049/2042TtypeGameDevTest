using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI valueText;

    private static readonly Color EmptyColor = new Color(0.75f, 0.75f, 0.75f, 0.35f);
    private static readonly Color FilledStartColor = new Color(0.85f, 0.85f, 0.85f, 1f);
    private static readonly Color FilledEndColor = new Color(1f, 0.7f, 0.2f, 1f);

    public void SetValue(int value)
    {
        if (value <= 0)
        {
            if (valueText != null)
            {
                valueText.text = "";
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = EmptyColor;
            }

            return;
        }

        if (valueText != null)
        {
            valueText.text = value.ToString();
        }

        if (backgroundImage != null)
        {
            float t = Mathf.Clamp01(Mathf.Log(value, 2f) / 11f);
            backgroundImage.color = Color.Lerp(FilledStartColor, FilledEndColor, t);
        }
    }
}