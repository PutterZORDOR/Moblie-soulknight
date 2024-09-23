using UnityEngine;

public class Shop_For_Dialogue : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LookMe.instance.MoveTo(gameObject.transform);
            Dialogue.instance.SetDialogue("Shop Room");
            gameObject.SetActive(false);
        }
    }
}
