using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class ShadeController : MonoBehaviour
{
    public static ShadeController Instance;
    private Animator animator;
    Image shade;
    [SerializeField]
    Color deathColor;
    [SerializeField]
    Color teleportColor;

    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        deathColor = Color.red;
        teleportColor = Color.white;
        animator = GetComponent<Animator>();
        shade = GetComponentInChildren<Image>();
    }

    public void ShowGUI()
    {
        CanvasManager.Instance.ShowGUI();
    }

    public void HideGUI()
    {
        CanvasManager.Instance.HideGUI();
    }

    public void ColorShade()
    {
        animator.SetBool("Transparent", false);
        animator.SetBool("Color", true);
    }

    public void ResetShade()
    {
        animator.SetTrigger("Reset");
    }

    public void TransparentShade()
    {
        animator.SetBool("Transparent", true);
        animator.SetBool("Color", false);
    }

    public void SetDeathColor()
    {
        shade.color = deathColor;
    }

    public void SetTeleportColor()
    {
        shade.color = teleportColor;
    }

    public void TriggerHub()
    {
        SetTeleportColor();
        ColorShade();
        animator.SetTrigger("TeleportHub");
    }

    public void TeleportHub()
    {
        SceneHandler.Instance.onLoadHub = true;
        TransparentShade();
    }

    public void TriggerLevel()
    {
        SetTeleportColor();
        ColorShade();
        animator.SetTrigger("TeleportLevel");
    }

    public void TeleportLevel()
    {
        SceneHandler.Instance.onLoadLevel = true;
        TransparentShade();
    }
}
