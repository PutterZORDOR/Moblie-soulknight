using UnityEngine;

public class ChestRandom : MonoBehaviour
{
    public WeightedRandomList<GameObject> lootTable;

    public Transform itemHolder;
    public bool CanOpen = true;
    public bool CanGetItem;
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject Hand_player;
    public GameObject This_Item;

    private void Start()
    {
        itemHolder = transform.Find("Item_Holder");
        Hand_player = GameObject.Find("Handle_Item");
        GetButton = transform.Find("Get_Button").gameObject;
        GetButton.SetActive(false);
    }

    private void Update()
    {
        if (CanGetItem || CanOpen)
        {
            IsPlayerInRange();
        }
    }

    public void OpenChest()
    {
        if (CanOpen)
        {
            Debug.Log("open");
            ShowItem();
            CanOpen = false;
            CanGetItem = true;
        }
        else
        {
            Debug.Log("Item ++");
            CanGetItem = false;
            HideItem();
            MoveItemToHand();
        }
    }

    public void MoveItemToHand()
    {
        if (This_Item != null)
        {
            This_Item.transform.SetParent(Hand_player.transform);
            This_Item.transform.localPosition = Vector3.zero;
        }
    }

    public void HideItem()
    {
        GetButton.SetActive(false);
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
                return true;
            }
        }
        GetButton.SetActive(false);
        return false;
    }

    void ShowItem()
    {
        itemHolder.localScale = Vector3.one;
        GameObject item = lootTable.GetRandom();
        This_Item = Instantiate(item, itemHolder);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


