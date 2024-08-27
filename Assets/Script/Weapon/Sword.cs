using UnityEngine;
using System.Collections;

public class Sword : Weapon
{
    private Animator SwordAnim;

    private void Start()
    {
        SwordAnim = GetComponent<Animator>();
    }
    protected override void Attack()
    {
        SwordAnim.SetBool("isAttacking",true);
        StartCoroutine(ResetAttackBool());
    }
    private IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(attackRate); // รอจนกว่าเวลาการโจมตีจะสิ้นสุด
        if (SwordAnim != null)
        {
            SwordAnim.SetBool("isAttacking", false);
        }
    }
}