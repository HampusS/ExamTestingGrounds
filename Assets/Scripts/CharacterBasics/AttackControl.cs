using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    public GameObject target;
    float range;
    float damage;
    float attackRate;

    float coolDown;

    // Use this for initialization
    void Start()
    {
        range = 5;
        damage = 10;
        attackRate = 0.25f;
        coolDown = attackRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && coolDown >= attackRate)
            Attack();
        else if (coolDown < attackRate)
            coolDown += Time.deltaTime;
    }

    void Attack()
    {
        coolDown = 0;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        Debug.Log(distance);
        if (distance <= range)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            float scalar = Vector3.Dot(direction, transform.forward);
            if (scalar > 0)
                target.GetComponent<HealthControl>().AdjustHealth(damage);
        }
    }
}
