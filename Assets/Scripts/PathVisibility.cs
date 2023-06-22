using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisibility : MonoBehaviour
{
    private Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.WaveOngoing)
        {
            objectRenderer.enabled = false;
        }
        else
        {
            objectRenderer.enabled = true;
        }
    }
}
