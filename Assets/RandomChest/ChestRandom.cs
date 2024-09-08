using System.Collections;
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
    public Animator anim;
    Weapon_Item item;
    DropItem drop;
    GameObject player;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
            anim.Play("OpenChest");
            CanOpen = false;
            UI_Getitem.SetActive(false);
            GetButton.SetActive(false);
            StartCoroutine(WaitForAnimation());
        }
    }
    private IEnumerator WaitForAnimation()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        ShowItem();
        this.enabled = false;
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
        item = lootTable.GetRandom();
        This_Item = Instantiate(item.gamePrefab, itemHolder);
        drop = This_Item.GetComponent<DropItem>();
        SetItemData();
        

        sizeItem = item.gamePrefab.transform.localScale;
        This_Item.transform.localScale = sizeItem * 0.5f;

    }
    private void SetItemData()
    {
        drop.weaponName = item.itemName;
        drop.Dmg = item.Damgae;
        drop.Cost = item.Mana;
        drop.AtkSpeed = item.AtkSpeed;
        drop.raritys = item.rarity;
        drop.type_weapon = item.weaponType;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


