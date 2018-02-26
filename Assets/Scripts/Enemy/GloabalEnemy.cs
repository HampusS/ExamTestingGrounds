using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloabalEnemy : MonoBehaviour {
    public static Vector3 goalPos;
    public static Vector3 startpos;
    static int nrOfEnemies = 30;

    public GameObject enemyPref;
    public Transform playerpos;


    public static int startPool = 5; 
    public static GameObject[] enemies = new GameObject[nrOfEnemies];

    void Start () {
        for (int i = 0; i < nrOfEnemies; i++)
        {
            Vector3 pos = new Vector3(transform.position.x +(Random.Range(-startPool, startPool)), 1, transform.position.z + (Random.Range(-startPool, startPool)));
            enemies[i] = (GameObject) Instantiate(enemyPref, pos, Quaternion.identity);
        }
	}
	void Update () {
        startpos = transform.position;
        goalPos = playerpos.transform.position + new Vector3(0,Random.Range(0,3),0);
    }
}
