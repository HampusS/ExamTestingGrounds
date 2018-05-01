using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform target;
    public GameObject enemy;
    public int Capacity;
    public float spawnRate;
    float time;
    public int amount { get; set; }

    private void Start()
    {
        amount = 1;
    }

    void Update()
    {
        if (amount < Capacity)
        {
            time += Time.deltaTime;
            if (time >= spawnRate)
            {
                time = 0;
                SpawnAgent();
            }
        }
    }

    void SpawnAgent()
    {
        amount++;
        // Improve spawn grounds to check if the ground is available
        float x = Random.Range(target.position.x - 10, target.position.x + 10);
        float z = Random.Range(target.position.z - 10, target.position.z + 10);
        /*GameObject clone = */
        Instantiate(enemy, new Vector3(x, target.position.y, z), enemy.transform.rotation);
        enemy.GetComponent<SpawnController>().spawner = this;
    }

}
