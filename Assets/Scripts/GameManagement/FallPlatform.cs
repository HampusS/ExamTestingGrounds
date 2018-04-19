using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour {

    private Animator anim;
	void Start () {
        anim = GetComponent<Animator>();
    }
    public void Fall()
    {
        anim.SetTrigger("fall");
    }
}
