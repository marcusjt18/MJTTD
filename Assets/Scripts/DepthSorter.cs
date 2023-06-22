using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSorter : MonoBehaviour
{
    void Start()
    {
        // First, try to get the SpriteRenderer from the current GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // If the SpriteRenderer was not found, then try to get it from the children
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        // Check if the SpriteRenderer is found to prevent NullReferenceExceptions
        if (spriteRenderer != null)
        {
            float yPos = transform.position.y;
            spriteRenderer.sortingOrder = -(int)(yPos);
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on this GameObject or its children.");
        }
    }
}


