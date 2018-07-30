﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBase : WeaponBase {

    protected override void Start()
    {
        base.Start();
        weaponType = WeaponType.Range;
    }

    public override void Execute()
    {
        animator.SetTrigger("ExecuteAttack");
        animator.speed = 1 - attackRate;
    }

    public override void SetWeaponType()
    {
        animator.SetBool("Range", true);
    }

    public override void ResetWeaponType()
    {
        animator.SetBool("Range", false);
    }

    public override void Running()
    {
        animator.SetBool("Run", true);
    }

    public override void StopRunning()
    {
        animator.SetBool("Run", false);
    }

    public override void Sheath()
    {
        animator.SetBool("Sheath", true);
    }

    public override void Unsheath()
    {
        animator.SetBool("Sheath", false);
    }

    public override void EnableHitbox()
    {
        base.EnableHitbox();
    }

    public override void DisableHitbox()
    {
        base.DisableHitbox();
    }

    public override void ResetAnimationSpeed()
    {
        base.ResetAnimationSpeed();
    }
}
