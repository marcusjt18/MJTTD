using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PooledText : MonoBehaviour
{
    protected TMPro.TextMeshProUGUI textMesh;

    public void Awake()
    {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public virtual void Initialize(string text, Vector3 position)
    {
        textMesh.text = text;
        transform.position = position;
    }
}

