using UnityEngine;
using TMPro;

public class TalentTooltip : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text description;
    [SerializeField]
    private TMP_Text allocated;

    private RectTransform rectTransform; // Reference to the RectTransform component

    public void Initialize() // Add an Initialize method
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Show(Vector2 position) // Takes in the position of where to show this
    {
        if (rectTransform == null) // Check if rectTransform is null
        {
            Initialize();
        }

        rectTransform.anchoredPosition = position; // Set the position of the tooltip
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetTooltipText(string name, string description, bool activated)
    {
        this.title.text = name;
        this.description.text = description;
        UpdateAllocatedText(activated);

    }

    public void UpdateAllocatedText(bool activated)
    {
        if (activated)
        {
            allocated.text = "Allocated";
            allocated.color = new Color(0.7f, 1f, 0.345f, 1f);
        }
        else
        {
            allocated.text = "Not allocated";
            allocated.color = new Color(1f, 0.345f, 0.345f, 1f);
        }
    }
}

