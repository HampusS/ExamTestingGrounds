using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessing : MonoBehaviour {

    public PostProcessingProfile postProfileMain;
    public PostProcessingProfile postProfileMainNoFx;
    PostProcessingProfile currentState;

    void Start()
    {
        currentState = GetComponent<PostProcessingBehaviour>().profile = postProfileMain;
    }

}
