using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMove : MonoBehaviour
{
    [SerializeField]
    Transform target;

    NavMeshAgent navMesh;

    // Use this for initialization
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetDestination()
    {
        navMesh.SetDestination(target.position);
    }
}
