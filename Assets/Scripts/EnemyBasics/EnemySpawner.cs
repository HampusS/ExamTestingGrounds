using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
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
        float x = Random.Range(player.position.x - 10, player.position.x + 10);
        float z = Random.Range(player.position.z - 10, player.position.z + 10);
        /*GameObject clone = */
        Instantiate(enemy, new Vector3(x, player.position.y, z), enemy.transform.rotation);
    }

}
