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

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        audioM = FindObjectOfType<AudioM>();
    }

    public abstract void Execute();
    public abstract void Running();
    public abstract void StopRunning();
    public abstract void Sheath();
    public abstract void Unsheath();

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
