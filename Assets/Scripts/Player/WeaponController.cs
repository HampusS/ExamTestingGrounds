using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    PlayerController player;
    WeaponBase currWeapon;
    bool reset = false;
    public bool enable { get; set; }

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        currWeapon = FindObjectOfType<WeaponBase>();
        enable = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currWeapon.ExecuteWeapon();
            }

            if (player.HideWeapon && !reset)
            {
                currWeapon.SheathWeapon();
                reset = true;
            }
            else if (!player.HideWeapon && reset)
            {
                currWeapon.UnsheathWeapon();
                reset = false;
            }

            if (player.RunWeapon)
                currWeapon.Running();
            else
                currWeapon.StopRunning();
        }
    }

    public void EnableHitbox()
    {
        currWeapon.boxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        currWeapon.boxCollider.enabled = false;
    }
    // SELECT WEAPON

    // UNLOCK WEAPON

}
