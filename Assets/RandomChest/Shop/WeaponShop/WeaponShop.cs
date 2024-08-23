using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    public WeightedRandomList lootTable;

    [Header("Price")]
    public int minPrice;
    public int maxPrice;

    public bool CanBuy = true;
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject This_Item;
    public GameObject UI_Buy;
    private Vector3 sizeItem;
    [SerializeField] int current_price;
    [SerializeField] Weapon_Item item;
    [SerializeField] DropItem drop;

    [Header("UI")]
    public TextMeshProUGUI text;

    [Header("UI Description")]
    public GameObject Description;
    public TextMeshProUGUI textDmg;
    public TextMeshProUGUI textAtkSpeed;
    public TextMeshProUGUI textCost;

    private void Start()
    {
        current_price = Random.Range(minPrice, maxPrice) * DungeonSystem.instance.Level;
        playerLayer = LayerMask.GetMask("Player");
        GetButton = transform.Find("Get_Button")?.gameObject;
        UI_Buy = transform.Find("UI_Buy")?.gameObject;
        text = UI_Buy.GetComponentInChildren<TextMeshProUGUI>();
        UI_Buy.SetActive(false);
        Description.SetActive(false);
        GetButton.SetActive(false);
        ShowItem();
    }

    private void SetDescription(Weapon_Item weapon)
    {
        textDmg.text = weapon.Damgae.ToString();
        textAtkSpeed.text = weapon.AtkSpeed.ToString();
        textCost.text = weapon.Mana.ToString();
    }

    private void Update()
    {
        if (CanBuy)
        {
            IsPlayerInRange();
        }
    }

    public void Buy()
    {
        if (CanBuy && CoinManager.instance.Coins >= current_price)
        {
            Debug.Log("Buy");
            drop.enabled = true;
            CoinManager.instance.SpendCoins(current_price);
            CanBuy = false;
            UI_Buy.SetActive(false);
            Description.SetActive(false);
            GetButton.SetActive(false);
            this.enabled = false;
        }
        else
        {
            Debug.Log("No Money");
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
                Description.SetActive(true);
                UI_Buy.SetActive(true);
                return true;
            }
        }
        UI_Buy.SetActive(false);
        Description.SetActive(false);
        GetButton.SetActive(false);
        return false;
    }
    void ShowItem()
    {
        item = lootTable.GetRandom();
        This_Item = Instantiate(item.gamePrefab, transform);
        SetDescription(item);
        switch (item.rarity)
        {
            case Rarity.Common:
                text.text = $"<sprite name=\"Coin\"> {current_price.ToString()} {item.itemName}";
                break;
            case Rarity.Uncommon:
                text.text = $"<sprite name=\"Coin\"> {current_price.ToString()} <color=#7CFC00>{item.itemName}</color>";
                break;
            case Rarity.Rare:
                text.text = $"<sprite name=\"Coin\"> {current_price.ToString()} <color=#48C9B0>{item.itemName}</color>";
                break;
            case Rarity.Epic:
                text.text = $"<sprite name=\"Coin\"> {current_price.ToString()} <color=#BA55D3>{item.itemName}</color>";
                break;
            case Rarity.Legendary:
                text.text = $"<sprite name=\"Coin\"> {current_price.ToString()} <color=#F7DC6F>{item.itemName}</color>";
                break;
            default:
                text.text = "Unknow";
                break;
        }
            drop = This_Item.GetComponent<DropItem>();
            SetItemData();
            StartCoroutine(CloseUI());
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

    IEnumerator CloseUI()
    {
        yield return new WaitForSeconds(0.5f);
        drop.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
