using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeManager : MonoBehaviour
{

    [SerializeField]
    GameObject fallPlatform;
    private Transform[] childs;
    private List<Transform> ChildList = new List<Transform>();
    int platformCount;
    void Start()
    {
        childs = GetComponentsInChildren<Transform>();
        //platformCount = childs.Length;
        for (int i = 1; i < childs.Length; i += 2)
        {
            ChildList.Add(childs[i]);
        }
    }
    void Update()
    {
        if (ChildList.Count == 0)
        {
            Fall();
        }
        for (int i = 0; i < ChildList.Count; i++)
        {
            if (ChildList[i].gameObject.GetComponent<CheckEmpty>().empty == true)
            {
                ChildList.RemoveAt(i);
            }
        }
    }
    void Fall()
    {
        fallPlatform.GetComponent<FallPlatform>().Fall();
    }
}
