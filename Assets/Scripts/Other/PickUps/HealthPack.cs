using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPack : MonoBehaviour
{
    PlayerController player;
    AudioM audioM;
    public Text canvasText;
    public float Amount;
    bool used;
    Color startColor;

    float timer = 1.5f, time;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        audioM = FindObjectOfType<AudioM>();
        if (canvasText != null)
            startColor = canvasText.color;
    }

    private void Update()
    {
        if (used)
        {
            time += Time.deltaTime;
            if (time >= timer)
            {
                if (canvasText != null)
                    canvasText.gameObject.SetActive(false);
                Destroy(gameObject);
            }
            if (canvasText != null)
                canvasText.color = new Color(canvasText.color.r, canvasText.color.g, canvasText.color.b, Mathf.PingPong(Time.time * 2, 1));
        }
    }

    public void PickedUp()
    {
        used = true;
        if (canvasText != null)
        {
            canvasText.gameObject.SetActive(true);
            canvasText.text = "+" + Amount;
            canvasText.color = startColor;
        }
        player.AddHealth(Amount);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        audioM.Play("healthpickup");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!used && other.tag == "Player")
        {
            PickedUp();
        }
    }
}
