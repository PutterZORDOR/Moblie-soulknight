using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public GameObject image;
    public GameObject back_Ground;
    public float changeInterval = 0.8f;
    Animator anim;

    public void Start()
    {
            anim = back_Ground.GetComponent<Animator>();
            image.SetActive(true);
            back_Ground.SetActive(true);
            loadingText.gameObject.SetActive(true);
            StartCoroutine(UpdateLoadingText());
    }

    IEnumerator UpdateLoadingText()
    {
        string baseText = "Loading";
        int dotCount = 0;
        int maxDots = 4;

        while (true)
        {
            loadingText.text = baseText + new string('.', dotCount);

            dotCount = (dotCount + 1) % (maxDots + 1);

            yield return new WaitForSeconds(changeInterval);
        }
    }

    public void StopLoading()
    {
        if (loadingText != null)
        {
            StopAllCoroutines();
            loadingText.text = "Load";
            loadingText.gameObject.SetActive(false);
            image.SetActive(false);
            anim.SetTrigger("Close");
        }
    }
}
