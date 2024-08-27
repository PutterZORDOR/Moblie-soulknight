using UnityEngine;

public class Sword : Weapon
{
    public LayerMask IsEnemy;  // Layer mask to identify enemies
    public int SwordDamage;
    public bool isInWeaponSlot = false;
    private void Start()
    {
        Damage = SwordDamage;
    }
    protected override void Attack()
    {
        // Detect enemies within the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, Range, IsEnemy);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Assuming the enemy has a method called TakeDamage
            enemy.GetComponent<Enemy>().TakeDamage(Damage);
        }
    }

    // Visualize the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;

        Gizmos.DrawWireSphere(AttackPoint.position, Range);
    }
}
