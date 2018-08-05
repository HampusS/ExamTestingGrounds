using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlickerHealth : MonoBehaviour
{
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color targetColor;
    float flickerSpeed = 4;

    public void FlickerHp()
    {
        CanvasManager.Instance.hpText.color = new Color(targetColor.r, targetColor.g, targetColor.b, Mathf.PingPong(Time.time * flickerSpeed, 1));
        CanvasManager.Instance.hpImg.color = CanvasManager.Instance.hpText.color;
    }

    public void ResetColor()
    {
        CanvasManager.Instance.hpText.color = startColor;
        CanvasManager.Instance.hpImg.color = startColor;
    }
}
