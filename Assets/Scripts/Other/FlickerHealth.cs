using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlickerHealth : MonoBehaviour
{
    [SerializeField]
    Image hpImg;
    [SerializeField]
    Text hpText;
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color targetColor;

    bool lowerIntensity;

    // Use this for initialization
    void Start()
    {
        hpText.color = startColor;
        hpImg.color = hpText.color;
    }

    public void FlickerHp()
    {
        if (lowerIntensity)
        {
            hpText.color = Color.Lerp(hpText.color, targetColor, Time.deltaTime * 40);
            hpImg.color = hpText.color;
            if (hpText.color == targetColor)
                lowerIntensity = false;
        }
        else if (!lowerIntensity)
        {
            hpText.color = Color.Lerp(hpText.color, startColor, Time.deltaTime * 40);
            hpImg.color = hpText.color;
            if (hpText.color == startColor)
                lowerIntensity = true;
        }
    }

    public void ResetColor()
    {
        hpText.color = startColor;
        hpImg.color = hpText.color;
    }
}
