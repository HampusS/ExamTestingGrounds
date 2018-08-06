using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    PlayerController player;
    WeaponBase currWeapon;
    [SerializeField]
    ThrowLogic throwLogic;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (currWeapon != null)
            {
                if (!player.HideWeapon && !player.LockMovement)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        currWeapon.Execute();
                    }
                    else if (Input.GetMouseButton(1) || throwLogic.Recall == true)
                    {
                        currWeapon.CustomExecute();
                    }
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
            if (weapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.Disk && throwLogic.Thrown)
                weapon.SetActive(true);

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
        AudioM.Instance.Play("launch");
    }

    public void DisableWeapon()
    {
        currWeapon.Disable();
    }

    public void EnableWeapon()
    {
        currWeapon.Enable();
    }

    public void Throw()
    {
        throwLogic.Throw();
    }

    public void Retrieve()
    {
        throwLogic.Retrieve();
    }
}
