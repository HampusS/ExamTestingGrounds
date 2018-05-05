using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnArea;
    public GameObject enemy;
    public int Capacity;
    public float spawnRate;
    float time;
    public int amount { get; set; }

    private void Start()
    {
        amount = 0;
        if (spawnArea == null)
            spawnArea = transform;
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
        float x = Random.Range(spawnArea.position.x - 10, spawnArea.position.x + 10);
        float z = Random.Range(spawnArea.position.z - 10, spawnArea.position.z + 10);
        GameObject clone = Instantiate(enemy, new Vector3(x, spawnArea.position.y, z), enemy.transform.rotation);
        clone.GetComponent<SpawnController>().spawner = this;
    }

}
