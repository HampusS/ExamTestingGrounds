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
    float flickerSpeed = 4;

    // Use this for initialization
    void Start()
    {
        hpText.color = startColor;
        hpImg.color = hpText.color;
    }

    public void FlickerHp()
    {
        hpText.color = new Color(targetColor.r, targetColor.g, targetColor.b, Mathf.PingPong(Time.time * flickerSpeed, 1));
        hpImg.color = hpText.color;
    }

    public void ResetColor()
    {
        hpText.color = startColor;
        hpImg.color = hpText.color;
    }
}
