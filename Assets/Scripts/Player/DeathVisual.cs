using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DeathVisual : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Dead()
    {
        anim.SetTrigger("Dead");
    }
    public void Alive()
    {
        anim.SetTrigger("Alive");
    }
}
