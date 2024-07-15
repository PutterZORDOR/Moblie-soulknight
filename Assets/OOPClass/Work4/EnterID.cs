using NUnit.Framework.Constraints;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnterID : MonoBehaviour
{
    public Text result;
    public int inputNum;
    public Work4 id;
    [SerializeField] InputField input;
    private void Start()
    {
        id = FindAnyObjectByType<Work4>();
    }

    public void ValidateInput()
    {
        string idNum = input.text;
        if (int.TryParse(idNum, out inputNum))
        {
            id.idnum = inputNum;
            id.ShowUp();
        }else
        {
            result.text = "Only Number";
            result.color = Color.red;
        }
        
    }
}
