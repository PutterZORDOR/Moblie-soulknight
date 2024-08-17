using UnityEngine;

public class ChestRandom : MonoBehaviour
{
    public WeightedRandomList lootTable;

    public Transform itemHolder;
    public bool CanOpen = true;
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject This_Item;
    public GameObject UI_Getitem;
    private Vector3 sizeItem;

    private void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        itemHolder = transform.Find("Item_Holder");
        GetButton = transform.Find("Get_Button").gameObject;
        UI_Getitem = transform.Find("UI_GetItem").gameObject;
        UI_Getitem.SetActive(false);
        GetButton.SetActive(false);
    }
    private void Update()
    {
        if (CanOpen)
        {
            IsPlayerInRange();
        }
    }

    public void OpenChest()
    {
        if (CanOpen)
        {
            Debug.Log("open");
            CanOpen = false;
            UI_Getitem.SetActive(false);
            GetButton.SetActive(false);
            ShowItem();
            this.enabled = false;
        }
    }
    bool IsPlayerInRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("Found");
                GetButton.SetActive(true);
                UI_Getitem.SetActive(true);
                return true;
            }
        }
        UI_Getitem.SetActive(false);
        GetButton.SetActive(false);
        return false;
    }
    void ShowItem()
    {
        itemHolder.localScale = Vector3.one;
        Weapon_Item item = lootTable.GetRandom();
        This_Item = Instantiate(item.gamePrefab, itemHolder);

        sizeItem = item.gamePrefab.transform.localScale;
        This_Item.transform.localScale = sizeItem * 2f;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


