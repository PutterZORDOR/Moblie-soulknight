using UnityEngine;
using UnityEngine.UIElements;

public class CloseBG : MonoBehaviour
{
    public GameObject UI;
    public void ColseBG()
    {
        Dialogue.instance.dialogue.SetActive(false);
        gameObject.SetActive(false);
        UI.SetActive(false);
    }
}
