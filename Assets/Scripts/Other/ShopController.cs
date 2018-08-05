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

    PlayerController player;
    bool interact;
    CameraControls cam;

    GameObject weaponSystem;
    Camera cameraMain;

    public static ShopController Instance;

    // Use this for initialization
    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        player = temp.GetComponent<PlayerController>();
        cam = temp.GetComponentInChildren<CameraControls>();
        weaponSystem = GameObject.Find("WeaponSystem");
        cameraMain = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, shopKeeper.position);
        if (distance > 100)
        {
            Vector3 newPos = new Vector3(shopKeeper.position.x - player.transform.position.x, 0, shopKeeper.position.z - player.transform.position.z).normalized;
            player.transform.position = shopKeeper.position + (newPos * 99);
            //player.transform.position += Vector3.up * 0.05f;
            //player.transform.rotation = Quaternion.LookRotation(-newPos);
        }
        if (CanvasManager.Instance.GUI)
        {
            CanvasManager.Instance.HideGUI();
        }
    }

    void ActivateShop(bool activate)
    {
        interact = activate;
        player.LockMovement = activate;
        cam.Disable = activate;
        CanvasManager.Instance.ShopCanvas.SetActive(activate);
        Cursor.visible = activate;
    }

    void InputShop()
    {
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Return))
        {
            ActivateShop(true);
            weaponSystem.transform.parent = cameraMain.transform.parent;
            Cursor.lockState = CursorLockMode.None;
            CanvasManager.Instance.HideGUI();
        }
        else if (Input.GetKey(KeyCode.Q) ||
            Input.GetKey(KeyCode.Escape) ||
            Input.GetMouseButton(1))
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
                if (cameraMain.transform.position != PlayerController.Instance.shopPivot.position)
                    cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, PlayerController.Instance.shopPivot.position, Time.deltaTime * 10);

                Vector3 target = lookHere.position - cameraMain.transform.position;
                RotateTarget(-target, shopKeeper);
                cam.TiltCameraUpDown(target);

                Vector3 playerDirection = lookHere.position - other.transform.position;
                RotateTarget(playerDirection, other.transform);
            }
            else
            {
                Vector3 target = other.transform.position - lookHere.position;
                RotateTarget(target, shopKeeper);
                if (cameraMain.transform.localPosition != Vector3.zero)
                    cameraMain.transform.localPosition = Vector3.Lerp(cameraMain.transform.localPosition, Vector3.zero, Time.deltaTime * 10);
                else
                    weaponSystem.transform.parent = cameraMain.transform;
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

    public void EnterSystem()
    {
        ExitShop();
        ShadeController.Instance.TriggerLevel();
        PlayerController.Instance.AddHealth(100);
    }

    public void ShowUpgrades()
    {
        CanvasManager.Instance.UpgradesCanvas.SetActive(true);
    }

    public void ExitShop()
    {
        ActivateShop(false);
        CanvasManager.Instance.UpgradesCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        cameraMain.transform.localPosition = Vector3.zero;
        CanvasManager.Instance.ShowGUI();
    }
}
