using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject agent;
    public int Capacity;
    public float spawnRate;
    float time;
    public int amount { get; set; }

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
        GameObject clone = Instantiate(agent, agent.transform.position, agent.transform.rotation);
    }
    
}
