using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleImpact : MonoBehaviour {


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            Vector3 direction = (transform.position - collision.transform.position).normalized;
            PlayerController.Instance.KnockBack(direction, 50);
            PlayerController.Instance.DamagePlayer(10);
        }
    }
}
