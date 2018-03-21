using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;
    public float maxSpeed = 10;
    public float minSpeed = 2.5f;
    public float rotationSpeed = 2.0f;
    public float neighbourDist = 2;
    public float perceptionArea = 10;
    // Use this for initialization
    Vector3 goal;
    bool turning = false;
    void Start()
    {
        speed = 10;//Random.Range(maxSpeed, minSpeed);

    }
    // Update is called once per frame
    void Update()
    {
        goal = GloabalEnemy.goalPos;
        if (Vector3.Distance(transform.position, goal) >= GloabalEnemy.startPool)
            turning = true;
        else
            turning = false;

        if (turning)
        {
            Vector3 dir = goal - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();

        }
        //if (Vector3.Distance(transform.position, goal) > 1)
        //{
        //    Vector3 dir = goal;
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
        //}
        
            transform.Translate(0, 0, Time.deltaTime * speed);
    }
    void ApplyRules()
    {
        GameObject[] enemyFlock;
        enemyFlock = GloabalEnemy.enemies;
        Vector3 vCenter = GloabalEnemy.startpos; //points to center of group
        Vector3 vAvoid = GloabalEnemy.startpos;//points away from group
        float gSpeed = 0.1f;


        float dist;

        int gorupSize = 0;
        foreach (GameObject e in enemyFlock)
        {
            if (e != this.gameObject)
            {
                dist = Vector3.Distance(e.transform.position, this.transform.position);
                if (dist <= neighbourDist)
                {
                    vCenter += e.transform.position;
                    gorupSize++;
                    if (dist < 15.0f)
                    {
                        vAvoid = vAvoid + (this.transform.position - e.transform.position);
                    }
                    Flock anotherFlock = e.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if (gorupSize > 0)
        {
            vCenter = vCenter / gorupSize + (goal - this.transform.position);
            speed = gSpeed / gorupSize;
            Vector3 dir = (vCenter + vAvoid) - transform.position;
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(dir),
                                                      rotationSpeed * Time.deltaTime);
            }
        }

    }
}
