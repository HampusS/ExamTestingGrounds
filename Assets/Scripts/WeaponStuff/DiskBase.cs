using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskBase : WeaponBase
{

    protected override void Start()
    {
        base.Start();
        weaponType = WeaponType.Disk;
    }

    public override void CustomExecute()
    {
        if (ThrowLogic.Instance.Thrown)
        {
            if (!ThrowLogic.Instance.Recall)
                FindObjectOfType<AudioM>().Play("recall");
            ThrowLogic.Instance.Recall = true;
            animator.SetBool("RecallDisk", true);
        }
        else
        {
            animator.SetBool("RecallDisk", false);
            ThrowLogic.Instance.Recall = false;
            DisableHitbox();
        }
    }

    public override void Execute()
    {
        animator.SetTrigger("ExecuteAttack");
        animator.speed = 1 - attackRate;
        EnableHitbox();
    }

    public override void SetWeaponType()
    {
        animator.SetBool("Disk", true);
    }

    public override void ResetWeaponType()
    {
        animator.SetBool("Disk", false);
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

    public override void Disable()
    {
        if (!ThrowLogic.Instance.Thrown)
            base.Disable();
    }

    public override void Enable()
    {
        base.Enable();
    }
}
