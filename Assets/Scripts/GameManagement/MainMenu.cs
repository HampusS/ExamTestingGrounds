using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public InputField name;
    public InputField id;
    int nrOfScenes;
    List<int> orderList = new List<int>();
    public int[] order = new int[4];
    int s1, s2, s3, s4;

    void Start()
    {
        nrOfScenes = SceneManager.sceneCountInBuildSettings;
        Debug.Log(nrOfScenes);
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
            int pick = Random.Range(0, orderList.Count-1);
            int sNr = orderList[pick];
            order[i] = sNr;
            orderList.RemoveAt(pick);
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void WritetToTxt()
    {
        Debug.Log("in write method");
        string nameStirng, idString;
        nameStirng = name.text;
        idString = id.text;
        //if (name.text != null && id.text != null)
        //{
            System.IO.File.AppendAllText("@/../../ExamTestingGrounds/test.txt", 
                "Tester name: "+ nameStirng+ System.Environment.NewLine+
                "Tester id: " + idString + System.Environment.NewLine +
                "The test order for this player was: " + 
                (order[0]-1).ToString()+ (order[1]-1).ToString()+ (order[2]-1).ToString()+ (order[3]-1).ToString()
                +System.Environment.NewLine +
                "-----------------------------------------------------------------------------------------------" + 
                System.Environment.NewLine);
        //}
    }
}
