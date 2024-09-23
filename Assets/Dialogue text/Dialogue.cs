using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class RoomDialogue
{
    public string roomName;
    public string[] lines;
}

public class Dialogue : MonoBehaviour
{
    [Header("Dialogue Pannel")]
    public GameObject dialogue;

    public List<RoomDialogue> rooms = new List<RoomDialogue>();
    public static Dialogue instance;
    public TextMeshProUGUI text;
    public float textSpeed;

    private int index;
    private string[] currentLine;
    private Dictionary<string, RoomDialogue> dialogueDictionary = new Dictionary<string, RoomDialogue>();

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

        foreach (var room in rooms)
        {
            dialogueDictionary[room.roomName] = room;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (text.text == currentLine[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = currentLine[index];
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
        foreach (char c in currentLine[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < currentLine.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogue.SetActive(false);
            LookMe.instance.BackToPlayer();
        }
    }

    public void SetDialogue(string roomName)
    {
        dialogue.SetActive(true);
        if (dialogueDictionary.TryGetValue(roomName, out RoomDialogue selectedRoom))
        {
            currentLine = selectedRoom.lines;
            text.text = string.Empty;
            StartDialogue();
        }
        else
        {
            Debug.LogWarning("Room type not found.");
        }
    }
}
