using UnityEngine;
using System.Collections;

public class FloatingGoldText : PooledText
{

    private bool shouldStartFadeOutAndMoveUp;

    public override void Initialize(string text, Vector3 position)
    {
        base.Initialize(text, position);
        shouldStartFadeOutAndMoveUp = true;
    }

    void Update()
    {
        if (shouldStartFadeOutAndMoveUp)
        {
            StartCoroutine(FadeOutAndMoveUp());
            shouldStartFadeOutAndMoveUp = false;
        }
    }

    IEnumerator FadeOutAndMoveUp()
    {
        float duration = 2.0f;
        float startTime = Time.time;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 2, 0); // Moves up by 2 units

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        // Once finished, return the text object to the pool
        TextPool.Instance.ReturnToPool("FloatingGoldText", gameObject);
    }
}


