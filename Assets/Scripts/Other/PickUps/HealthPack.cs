using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPack : MonoBehaviour
{
    PlayerController player;
    public Text canvasText;
    public float Amount;
    bool used;
    Color startColor;

    float timer = 1.5f, time;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        startColor = canvasText.color;
    }

    private void Update()
    {
        if (used)
        {
            canvasText.color = new Color(canvasText.color.r, canvasText.color.g, canvasText.color.b, Mathf.PingPong(Time.time * 2, 1));

            time += Time.deltaTime;
            if (time >= timer)
            {
                canvasText.gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

    public void PickedUp()
    {
        used = true;
        canvasText.gameObject.SetActive(true);
        canvasText.text = "+" + Amount;
        canvasText.color = startColor;
        player.AddHealth(Amount);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!used && other.tag == "Player")
        {
            PickedUp();
        }
    }
}
