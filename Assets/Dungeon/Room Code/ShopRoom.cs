using UnityEngine;

public class ShopRoom : MonoBehaviour
{
    [Header("Shop System")]
    public GameObject ShopRoomPrefab;
    void Start()
    {
        Instantiate(ShopRoomPrefab,transform);
    }
}
