using UnityEngine;

public class ChestRoom : MonoBehaviour
{
    [Header("Chest Room")]
    public GameObject ChestPrefab;
    void Start()
    {
        Instantiate(ChestPrefab,transform);
    }
}
