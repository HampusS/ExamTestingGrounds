using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerController player;

    WeaponBase currWeapon;
    ThrowLogic thorwLogic;
    bool reset = false;
    public bool enable { get; set; }

    public int weaponIndex = 0;
    [SerializeField]
    List<GameObject> weapons;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        WeaponSelection();
        enable = true;
        currWeapon.GetComponent<WeaponBase>().SetWeaponType();
        thorwLogic = new ThrowLogic();
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (currWeapon != null)
            {
                if (Input.GetMouseButtonDown(0) && !player.HideWeapon && !player.LockMovement)
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
            }
            if (!player.LockMovement)
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

        if (previousWeapon != weaponIndex)
        {
            currWeapon.SwapWeapon();
        }
    }

    public void WeaponSelection()
    {
        int i = 0;
        foreach (GameObject weapon in weapons)
        {
            if (i == weaponIndex)
            {
                if (currWeapon != null)
                    currWeapon.ResetWeaponType();
                weapon.SetActive(true);
                currWeapon = weapon.GetComponent<WeaponBase>();
                currWeapon.SwappedWeapon();
                currWeapon.SetWeaponType();
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

    public void ResetAnimationSpeed()
    {
        currWeapon.ResetAnimationSpeed();
    }

    public void SpawnProjectile()
    {
        currWeapon.GetComponentInChildren<SpawnProjectile>().Spawn();
    }

    public void DisableWeapon()
    {
        currWeapon.gameObject.SetActive(false);
    }

    public void EnableWeapon()
    {
        currWeapon.gameObject.SetActive(true);
    }

    public void ThrowRecive()
    {
        thorwLogic.Update();
    }
}
