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
            JoystickMove joy = collision.GetComponent<JoystickMove>();
            joy.isStopped = true;
            GameObject p = GameObject.FindGameObjectWithTag("Interaction");
            p.SetActive(false);
            d.SetActive(true); 
            LookMe.instance.MoveTo(gameObject.transform);
            Dialogue.instance.SetDialogue("Shop Room");
            Collider2D col = gameObject.GetComponent<Collider2D>();
            col.enabled = false;
        }
    }
}
