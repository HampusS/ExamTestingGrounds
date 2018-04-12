using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    ERROR,
    IDLE,
    MOVE,
    ATTACK,
}

public class EnemyController : MonoBehaviour {

    protected EnemyStates tasks;
    public GameObject player;
    public Transform target;

	// Use this for initialization
	void Start () {
        tasks = EnemyStates.ERROR;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
