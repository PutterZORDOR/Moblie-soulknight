using UnityEngine;

public class Mana_Manager : MonoBehaviour
{
    public GameObject Mana;
    public GameObject[] ManaPool = new GameObject[15];
    void Start()
    {
        for (int i = 0; i < ManaPool.Length; i++)
        {
            ManaPool[i] = Instantiate(Mana,transform.position,Quaternion.identity);
            ManaPool[i].SetActive(false);
        }
    }
}
