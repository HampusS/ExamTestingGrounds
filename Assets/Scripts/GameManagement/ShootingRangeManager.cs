using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeManager : MonoBehaviour
{
    private Transform[] childs;
    private List<Transform> ChildList = new List<Transform>();
    int platformCount;
    bool done;

    [SerializeField]
    ElevatorControls elevator;

    void Start()
    {
        childs = GetComponentsInChildren<Transform>();
        for (int i = 1; i < childs.Length; i += 2)
        {
            ChildList.Add(childs[i]);
        }
    }

    void Update()
    {
        if (!done && ChildList.Count == 0)
        {
            done = true;
            elevator.Activated = true;
            GameObject[] cubes = GameObject.FindGameObjectsWithTag("GoalCubeTag");
            foreach (GameObject cube in cubes)
            {
                Destroy(cube, 1);
            }
        }
        for (int i = 0; i < ChildList.Count; i++)
        {
            if (ChildList[i].gameObject.GetComponent<CheckEmpty>().empty == true)
            {
                ChildList.RemoveAt(i);
            }
        }
    }
}
