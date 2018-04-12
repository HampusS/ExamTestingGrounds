using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SwarmerController controller;
    public GameObject agent;
    public int Capacity;
    public float spawnRate;
    float time;
    public int amount { get; set; }

    private void Start()
    {

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
        float x = Random.Range(controller.target.position.x - 10, controller.target.position.x + 10);
        float y = controller.target.position.y;
        GameObject clone = Instantiate(agent, new Vector3(x, y), agent.transform.rotation);
    }

}
