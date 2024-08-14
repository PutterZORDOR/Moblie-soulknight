using UnityEngine;

public class CenterEnemy : MonoBehaviour
{
    public int hp;
    public int atk;
    public float detectionRange = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CircleCollider2D detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
