using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    Transform shopKeeper;
    [SerializeField]
    Transform lookHere;
    [SerializeField]
    Transform shopPivot;

    [SerializeField]
    GameObject ShopCanvas;
    [SerializeField]
    GameObject UpgradesCanvas;

    [SerializeField]
    Text currencyText;

    PlayerController player;
    bool interact;
    CameraControls cam;

    GameObject canvas;
    [SerializeField]
    GameObject shopMenu;

    // Use this for initialization
    void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        player = temp.GetComponent<PlayerController>();
        cam = temp.GetComponentInChildren<CameraControls>();
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, shopKeeper.position);
        if(distance > 100)
        {
            Vector3 newPos = new Vector3(shopKeeper.position.x - player.transform.position.x, 0, shopKeeper.position.z - player.transform.position.z).normalized;
            player.transform.position = shopKeeper.position + (newPos * 99);
            //player.transform.position += Vector3.up * 0.05f;
            //player.transform.rotation = Quaternion.LookRotation(-newPos);
        }
    }

    void ActivateShop(bool activate)
    {
        interact = activate;
        player.LockMovement = activate;
        cam.Disable = activate;
        ShopCanvas.SetActive(activate);
        Cursor.visible = activate;
        player.HideWeapon = activate;
    }

    void InputShop()
    {
        if (Input.GetKey(KeyCode.E))
        {
            ActivateShop(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            ExitShop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            InputShop();
            if (interact)
            {
                currencyText.text = player.Currency.ToString();
                Camera camera = Camera.main;
                camera.transform.position = Vector3.Lerp(camera.transform.position, shopPivot.position, Time.deltaTime * 10);

                Vector3 target = lookHere.position - camera.transform.position;
                RotateTarget(-target, shopKeeper);
                cam.TiltCameraUpDown(target);

                Vector3 playerDirection = lookHere.position - other.transform.position;
                RotateTarget(playerDirection, other.transform);
            }
            else
            {
                Vector3 target = other.transform.position - lookHere.position;
                RotateTarget(target, shopKeeper);
                Camera camera = Camera.main;
                camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, Vector3.zero, Time.deltaTime * 10);
                //if(camera.transform.localPosition == Vector3.zero)

            }
        }
    }

    void RotateTarget(Vector3 target, Transform from)
    {
        float speed = Time.deltaTime * 3;
        Vector3 newDir = Vector3.RotateTowards(from.forward, target, speed, 0);
        newDir = Vector3.ProjectOnPlane(newDir, Vector3.up);
        from.rotation = Quaternion.LookRotation(newDir);
    }

    private void OnTriggerExit(Collider other)
    {
        ExitShop();
    }

    public void ShowUpgrades()
    {
        UpgradesCanvas.SetActive(true);
    }

    public void ExitShop()
    {
        ActivateShop(false);
        UpgradesCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
