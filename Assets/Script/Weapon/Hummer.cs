using UnityEngine;
using System.Collections;

public class Hummer : Weapon
{
    private Animator SmashAnim;
    public GameObject Lava; // Prefab ของลาวาที่จะสร้าง
    //public Transform LavaPoint;  // จุดที่ต้องการให้ลาวาปรากฏ

    private void Start()
    {
        SmashAnim = GetComponent<Animator>();
    }
    protected override void Attack()
    {
        SmashAnim.SetBool("isAttacking", true);
        StartCoroutine(ResetAttackBool());
        Instantiate(Lava, AttackPoint.position, Quaternion.identity);
    }
    private IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(attackRate); // รอจนกว่าเวลาการโจมตีจะสิ้นสุด
        if (SmashAnim != null)
        {
            SmashAnim.SetBool("isAttacking", false);
        }
    }
}
