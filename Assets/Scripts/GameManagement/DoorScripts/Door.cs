using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int nrOfLocks = 1;
    public int nrOfKeys { get; set; }
    public List<GameObject> keys = new List<GameObject>();
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        if (nrOfKeys == nrOfLocks)
        {
            anim.SetTrigger("Open");
        }
    }
}
