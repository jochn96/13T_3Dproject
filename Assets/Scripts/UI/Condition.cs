using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar; 

    private void Start()
    {
        curValue =  startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = Getpercentage();
    }

    float Getpercentage()
    {
        return curValue / maxValue; 
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);  //더 작은 것을 넣어라
    }

    public void Subject(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
