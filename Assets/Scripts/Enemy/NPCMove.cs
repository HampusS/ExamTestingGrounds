using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour {

    [SerializeField]
    Transform _dest;
    NavMeshAgent navAgent;
    
	void Awake ()
    {

        navAgent = this.GetComponent<NavMeshAgent>();
        if(navAgent == null)
        {
            Debug.LogError("No navmeshagent attached to NPC");
        }
        else
        {
            SetDest();
        }
	}
    void SetDest()
    {
        if(_dest != null)
        {
            Vector3 target = _dest.transform.position;
            navAgent.SetDestination(target);
        }

    }

	void Update ()
    {
        SetDest();
	}
}
