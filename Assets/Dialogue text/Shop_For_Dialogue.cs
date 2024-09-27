using UnityEngine;
using UnityEngine.InputSystem;

public class Shop_For_Dialogue : MonoBehaviour
{
    public GameObject d;
    private void Start()
    {
        d = GameObject.FindGameObjectWithTag("Dialogue");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (DungeonSystem.instance.shop_count == 0 || DungeonSystem.instance.Level == 15)
            {
                JoystickMove joy = collision.GetComponent<JoystickMove>();
                joy.isStopped = true;
                GameObject p = GameObject.FindGameObjectWithTag("Interaction");
                p.SetActive(false);
                d.SetActive(true);
                LookMe.instance.MoveTo(gameObject.transform);
                if (DungeonSystem.instance.shop_count == 0)
                {
                    Dialogue.instance.SetDialogue("Shop Room");
                }
                else if (DungeonSystem.instance.Level == 15)
                {
                    Dialogue.instance.SetDialogue("Shop15 Room");
                }
                Collider2D col = gameObject.GetComponent<Collider2D>();
                col.enabled = false;
                DungeonSystem.instance.shop_count++;
            }
        }
    }
}
