using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEmpty : MonoBehaviour
{
    int count;
    public Light spotlight;
    public Color color0;
    public Color color1;
    public float lerpFloat = 13;
    void ChangeLightColor()
    {
        if (isEmpty())// && spotlight.color != color1)
        {
            spotlight.color = color1;//Color.Lerp(color0, color1, Time.deltaTime*HampusFloat);
        }
    }
    bool isEmpty()
    {
        if (count <= 0)
        {
            return true;
        }
        return false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GoalCubeTag")
        {
            count++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "GoalCubeTag")
        {
            count--;
        }
        Debug.Log(count);
        if (isEmpty())
        {
            Invoke("ChangeLightColor", 3);
        }
    }

}