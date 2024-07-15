using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Work4 : MonoBehaviour
{
    public List<ID> ID_Student = new List<ID>();
    public Dictionary<int, string> ID_ = new Dictionary<int, string>();
    public int idnum;
    public EnterID ShowText;
    private void Awake()
    {
        ShowText = FindAnyObjectByType<EnterID>();
        foreach (ID i in ID_Student)
        {
            ID_.Add(i.id, i.Student_name);
        }
    }
    public void ShowUp()
    {
        if (ID_.TryGetValue(idnum, out string studentName))
        {
            ShowText.result.text = studentName;
            ShowText.result.color = Color.green;
        }
        else
        {
            ShowText.result.text = "NotFound";
            ShowText.result.color = Color.red;
        }
    }
}
