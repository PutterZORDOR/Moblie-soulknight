using UnityEngine;

public class Room_Map : MonoBehaviour
{
    [SerializeField]Color myColor;
    SpriteRenderer myRenderer;
    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myColor = myRenderer.color;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Icon"))
        {
            Debug.Log("T");
            myRenderer.color = Color.white;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Icon"))
        {
            Debug.Log("F");
            myRenderer.color = myColor;
        }
    }
}
