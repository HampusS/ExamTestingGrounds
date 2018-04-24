using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMScript : MonoBehaviour {
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
