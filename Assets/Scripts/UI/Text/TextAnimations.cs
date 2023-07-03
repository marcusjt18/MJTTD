using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnimations : MonoBehaviour
{
    public IEnumerator ShakeText(TMP_Text text, float duration, float strength)
    {
        if (text != null)
        {
            Vector3 originalPosition = text.transform.localPosition;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                float x = originalPosition.x + Mathf.Sin(elapsed * strength) * strength;
                text.transform.localPosition = new Vector3(x, originalPosition.y, originalPosition.z);

                elapsed += Time.deltaTime;
                yield return null; // wait for the next frame
            }

            // return to original position
            text.transform.localPosition = originalPosition;
        }
    }


    public IEnumerator ScaleTextUpAndDown(TMP_Text text, float duration)
    {
        float elapsed = 0;

        // Save the original scale
        Vector3 originalScale = text.transform.localScale;

        // Calculate the larger scale
        Vector3 largerScale = originalScale * 1.2f; // Change to how much you want to scale

        while (elapsed < duration / 2)
        {
            float t = elapsed / (duration / 2); // Calculates normalized time (between 0 and 1)
            text.transform.localScale = Vector3.Lerp(originalScale, largerScale, t);
            elapsed += Time.deltaTime;
            yield return null; // wait for the next frame
        }

        elapsed = 0;

        while (elapsed < duration / 2)
        {
            float t = elapsed / (duration / 2);
            text.transform.localScale = Vector3.Lerp(largerScale, originalScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the text scale is set back to original
        text.transform.localScale = originalScale;
    }
}

