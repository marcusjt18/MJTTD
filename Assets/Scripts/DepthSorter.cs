using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSorter : MonoBehaviour
{
    void Start()
    {
        UpdateOrder();
    }

    public void UpdateOrder()
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

            // Add a small offset based on x position
            spriteRenderer.sortingOrder = -(int)((yPos * 100));
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on this GameObject or its children.");
        }
    }


    public void UpdateOrderForMonster()
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
            float xPos = transform.position.x;

            // Add a small offset based on x position
            spriteRenderer.sortingOrder = -(int)((yPos * 100) - (GetComponent<Monster>().Speed*10) - 50);
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on this GameObject or its children.");
        }
    }


}




