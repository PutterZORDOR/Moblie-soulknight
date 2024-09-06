using UnityEngine;

public class DetectDropWeapon : MonoBehaviour
{
    public Vector2 checkSize = new Vector2(5.0f, 5.0f);
    public void DetectDrop()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, checkSize, 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Unequip"))
            {
                collider.gameObject.SetActive(false);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, checkSize);
    }
}
