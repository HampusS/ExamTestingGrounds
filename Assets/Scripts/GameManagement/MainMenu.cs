using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int nrOfScenes;
    List<int> orderList = new List<int>();
    public int[] order = new int[4];

    void Start()
    {
        nrOfScenes = SceneManager.sceneCountInBuildSettings;
        for (int i = 2; i < nrOfScenes; i++)
        {
            orderList.Add(i);
        }
        PickOrder();
    }

    void PickOrder()
    {
        for (int i = 0; i < nrOfScenes - 3; i++)
        {
            int pick = Random.Range(0, orderList.Count - 1);
            int sNr = orderList[pick];
            order[i] = sNr;
            orderList.RemoveAt(pick);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

}
