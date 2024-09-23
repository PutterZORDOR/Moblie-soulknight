using UnityEngine;
using UnityEngine.InputSystem;

public class Shop_For_Dialogue : MonoBehaviour
{
    public GameObject d;
    private void Start()
    {
        d = GameObject.FindGameObjectWithTag("Dialogue");
        d.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
            GameObject p = GameObject.FindGameObjectWithTag("Interaction");
            p.SetActive(false);
            d.SetActive(true); 
            LookMe.instance.MoveTo(gameObject.transform);
            Dialogue.instance.SetDialogue("Shop Room");
            gameObject.SetActive(false);
        }
    }
}
