using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollider : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            SceneHandler.Instance.IncreaseLevelIndex();
            ShadeController.Instance.TriggerHub();
        }
    }
}
