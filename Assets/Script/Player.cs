using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform enemy;

    void Update()
    {
        if (enemy != null)
        {
            Vector3 direction = enemy.position - transform.position;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }
}

