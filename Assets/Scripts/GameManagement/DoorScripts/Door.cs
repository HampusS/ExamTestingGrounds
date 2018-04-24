using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool locked = true;
    public bool haskey = false;
     Animator anim; 
    void Awake()
    {
       anim = GetComponent<Animator>();
    }

    public void Open()
    {
        Debug.Log(anim);
        anim.SetTrigger("Open");
    }
}
