using UnityEngine;
using System.Collections;

public class Sword : Weapon
{
    private Animator SwordAnim;
    private bool isAttacking = false;

    private void Start()
    {
        SwordAnim = GetComponent<Animator>();
    }

    protected override void Attack()
    {
        if (!isAttacking && isInWeaponSlot) 
        {
            isAttacking = true;
            SwordAnim.SetBool("isAttacking", true);
            StartCoroutine(ResetAttackBool());
        }
    }

    private IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(attackRate); 
        SwordAnim.SetBool("isAttacking", false);
        isAttacking = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Damage); 
                    Debug.Log("Enemy take damage");
                    
                }
            }
        }
    }
}
