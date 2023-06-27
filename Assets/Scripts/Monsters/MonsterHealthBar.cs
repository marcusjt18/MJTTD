using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{

    [SerializeField]
    private Slider slider;

    public void UpdateHealthbar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
        Debug.Log(slider.value + " VALUE");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
