using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBase : WeaponBase
{
    protected override void Start()
    {
        base.Start();
        weaponType = WeaponType.Melee;
    }

    public override void SetWeaponType()
    {
        animator.SetBool("Melee", true);
    }

    public override void ResetWeaponType()
    {
        animator.SetBool("Melee", false);
    }

    public override void Execute()
    {
        animator.SetTrigger("ExecuteAttack");
        animator.speed = 1 - attackRate;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyController temp = other.GetComponent<EnemyController>();
            temp.Damage(damage);
            temp.KnockBack(transform.position);
        }
    }

    public override void EnableHitbox()
    {
        audioM.Play("weaponswing");
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
