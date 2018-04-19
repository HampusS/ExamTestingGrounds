using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteText : MonoBehaviour {

    public string text;
    string meshString = "";
    TextMesh textMesh;
	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
        Debug.Log(text[3]);
        TextAnim(text);
    }
	
	// Update is called once per frame
	void Update () {
	}
    void TextAnim(string inputText)
    {
        for (int i = 0; i < inputText.Length; i++)
        {
            //new WaitForSeconds(544.5f);
            Invoke("AddLetter(" + inputText[i] + ")", 50);
        }
    }
    void AddLetter(char letter)
    {
            textMesh.text = textMesh.text + letter;

    }
}
