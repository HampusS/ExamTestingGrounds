using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour {
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected float attackRate;
    [SerializeField]
    protected float damage;
    protected BoxCollider boxCollider;
    protected AudioM audioM;

    public enum WeaponType
    {
        None,
        Sword,
        Disk,
        Gun,
    }
    public WeaponType weaponType;

    protected virtual void Start()
    {
        weaponType = WeaponType.None;
        boxCollider = GetComponent<BoxCollider>();
        audioM = FindObjectOfType<AudioM>();
    }

    public abstract void Execute();
    public abstract void Running();
    public abstract void StopRunning();
    public abstract void Sheath();
    public abstract void Unsheath();
    public abstract void SetWeaponType();
    public abstract void ResetWeaponType();

    public virtual void ResetAnimationSpeed()
    {
        animator.speed = 1;
    }

    public virtual void EnableHitbox()
    {
        boxCollider.enabled = true;
    }

    public virtual void DisableHitbox()
    {
        boxCollider.enabled = false;
    }

    public virtual void SwapWeapon()
    {
        animator.SetBool("Swap", true);
    }

    public virtual void SwappedWeapon()
    {
        animator.SetBool("Swap", false);
    }
}
