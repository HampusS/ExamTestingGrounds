using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    PlayerController player;

    WeaponBase currWeapon;
    bool reset = false;
    public bool enable { get; set; }

    public int weaponIndex = 0;
    [SerializeField]
    List<GameObject> weapons;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        WeaponSelection();
        enable = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (Input.GetMouseButtonDown(0) && !player.HideWeapon)
            {
                currWeapon.Execute();
            }

            if (player.HideWeapon && !reset)
            {
                currWeapon.Sheath();
                reset = true;
            }
            else if (!player.HideWeapon && reset)
            {
                currWeapon.Unsheath();
                reset = false;
            }

            if (player.isRunning)
                currWeapon.Running();
            else
                currWeapon.StopRunning();

            ScrollThroughWeapons();
        }
    }

    void ScrollThroughWeapons()
    {
        int previousWeapon = weaponIndex;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            if (weaponIndex >= weapons.Count - 1)
                weaponIndex = 0;
            else
                weaponIndex++;
        }
        else if (scroll < 0)
        {
            if (weaponIndex <= 0)
                weaponIndex = weapons.Count - 1;
            else
                weaponIndex--;
        }

        if(previousWeapon != weaponIndex)
        {
            //SHEATH CURRENT WEAPON AND AT THE END OF ANIMATION CALL FOR WEAPON SWITCH
            WeaponSelection();
        }
    }

    void WeaponSelection()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            if (i == weaponIndex)
            {
                weapon.SetActive(true);
                currWeapon = weapon.GetComponent<WeaponBase>();
            }
            else
                weapon.SetActive(false);
            i++;
        }
    }

    public void EnableHitbox()
    {
        currWeapon.EnableHitbox();
    }

    public void DisableHitbox()
    {
        currWeapon.DisableHitbox();
    }
}
