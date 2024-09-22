using UnityEngine;
using UnityEngine.UIElements;

public class CloseBG : MonoBehaviour
{
    public GameObject UI;
    public void ColseBG()
    {
        gameObject.SetActive(false);
        UI.SetActive(false);
    }
}
