using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{

    [SerializeField]
    private GameObject linePrefab;

    private Dictionary<(RectTransform, RectTransform), GameObject> lineDictionary = new Dictionary<(RectTransform, RectTransform), GameObject>();


    public void DrawLine(RectTransform rect1, RectTransform rect2, bool thickLine)
    {
        Vector2 position1 = rect1.anchoredPosition;
        Vector2 position2 = rect2.anchoredPosition;

        Vector2 centerPos = (position1 + position2) / 2f;
        GameObject line = Instantiate(linePrefab, transform);
        RectTransform rectTransform = line.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = centerPos;

        float distance = Vector2.Distance(position1, position2);
        rectTransform.sizeDelta = new Vector2(distance, thickLine ? 8f : 3f);

        float angle = Mathf.Atan2((position2.y - position1.y), position2.x - position1.x) * Mathf.Rad2Deg;
        rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

        Image lineImage = line.GetComponent<Image>();

        // Here we change the color of the line depending on whether it is thick or not
        if (thickLine)
        {
            lineImage.color = new Color(255 / 255f, 220 / 255f, 69 / 255f, 1); // convert from 0-255 range to 0-1 range for color
        }
        else
        {
            lineImage.color = Color.white; // or whatever the default color should be
        }

        lineDictionary.Add((rect1, rect2), line);
    }

    public void UpdateLineThickness(RectTransform rect1, RectTransform rect2, bool thickLine)
    {
        GameObject line;
        if (lineDictionary.TryGetValue((rect1, rect2), out line) || lineDictionary.TryGetValue((rect2, rect1), out line))
        {
            RectTransform rectTransform = line.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, thickLine ? 8f : 3f);

            Image lineImage = line.GetComponent<Image>();

            // Here we change the color of the line depending on whether it is thick or not
            if (thickLine)
            {
                lineImage.color = new Color(255 / 255f, 220 / 255f, 69 / 255f, 1); // convert from 0-255 range to 0-1 range for color
            }
            else
            {
                lineImage.color = Color.white; // or whatever the default color should be
            }
        }
    }


}
