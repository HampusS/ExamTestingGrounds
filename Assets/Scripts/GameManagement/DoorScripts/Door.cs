using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public int nrOfLocks = 1;
    public bool locked = true;
    public bool haskey = false;
    public int currentKeys { get; set; }
    public List<GameObject> keys = new List<GameObject>();

    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        if (currentKeys == nrOfLocks)
        {
            locked = false;
            anim.SetTrigger("Open");
        }
    }
}
