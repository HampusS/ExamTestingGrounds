using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {
    public Animator animator;
    public BoxCollider boxCollider;
    public float attackRate;
    public float damage;

    public void ExecuteWeapon()
    {
        boxCollider.enabled = true;
        animator.SetTrigger("Execute");
        animator.speed = 1 - attackRate;
    }
    
    public void ResetSpeed()
    {
        animator.speed = 1;
    }

    public void Running()
    {
        animator.ResetTrigger("StopRunning");
        animator.SetTrigger("Running");
    }

    public void StopRunning()
    {
        animator.ResetTrigger("Running");
        animator.SetTrigger("StopRunning");
    }

    public void SheathWeapon()
    {
        animator.SetTrigger("Sheath");
        animator.ResetTrigger("Unsheath");
    }

    public void UnsheathWeapon()
    {
        animator.SetTrigger("Unsheath");
        animator.ResetTrigger("Sheath");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            EnemyController temp = other.GetComponent<EnemyController>();
            temp.Damage(damage);
            temp.KnockBack(transform.position);
        }
    }
}
