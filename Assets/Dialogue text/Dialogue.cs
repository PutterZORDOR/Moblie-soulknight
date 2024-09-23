using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public static Dialogue instance;
    public TextMeshProUGUI text;
    public string[] line1;
    public string[] line2;
    public float textSpeed;

    private int index;
    private string[] currentline;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (DungeonSystem.instance.Level == 1)
        {
            currentline = new string[line1.Length];
        }
        else if(DungeonSystem.instance.Level == 15)
        {

        }
        text.text = string.Empty;
        StartDialogue();
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(text.text == currentline[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = currentline[index];
            }
        }
    }
    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach(char c in currentline[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if(index < currentline.Length - 1) 
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }else
        {
            gameObject.SetActive(false);
        }
    }
}
