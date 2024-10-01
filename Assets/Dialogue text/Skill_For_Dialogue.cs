using UnityEngine;

public class Skill_For_Dialogue : MonoBehaviour
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
                JoystickMove joy = collision.GetComponent<JoystickMove>();
                joy.isStopped = true;
                GameObject p = GameObject.FindGameObjectWithTag("Interaction");
                p.SetActive(false);
                d.SetActive(true);
                LookMe.instance.MoveTo(gameObject.transform);
                if (DungeonSystem.instance.Level == 15)
                {
                    Dialogue.instance.SetDialogue("Skill15 Room");
                }
                else
                {
                    Dialogue.instance.SetDialogue("Skill Room");
                }
                Collider2D col = gameObject.GetComponent<Collider2D>();
                col.enabled = false;
        }
    }
}
