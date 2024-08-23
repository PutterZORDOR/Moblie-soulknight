using TMPro;
using UnityEngine;

public class PotionRandom : MonoBehaviour
{
    public WeightPotion lootTable;

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
    [SerializeField]All_Potion item;

    [Header("UI")]
    public TextMeshProUGUI text;

    [Header("UI Description")]
    public GameObject Description;
    public TextMeshProUGUI textPotion;
    private void Start()
    {
        item = lootTable.GetRandom();
        current_price = Random.Range(minPrice, maxPrice) * DungeonSystem.instance.Level;
        playerLayer = LayerMask.GetMask("Player");
        GetButton = transform.Find("Get_Button").gameObject;
        Description = transform.Find("Description_Potion").gameObject;
        textPotion = Description.GetComponentInChildren<TextMeshProUGUI>();
        UI_Buy = transform.Find("UI_Buy").gameObject;
        text = UI_Buy.GetComponentInChildren<TextMeshProUGUI>();
        UI_Buy.SetActive(false);
        Description.SetActive(false);
        GetButton.SetActive(false);
        ShowItem();
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
            CoinManager.instance.SpendCoins(current_price);
            CanBuy = false;
            UI_Buy.SetActive(false);
            Description.SetActive(false);
            GetButton.SetActive(false);
            if (item != null)
            {
                if (item.Type == Type_Potion.Heal)
                {
                    PlayerManager.instance.Heal(item.potionEff);
                }
                else if (item.Type == Type_Potion.Mana)
                {
                    PlayerManager.instance.RecoverMana(item.potionEff);
                }
            }
            This_Item.SetActive(false);
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
        if (item != null)
        {
            if (item.Type == Type_Potion.Heal)
            {
                text.text = $"<sprite name=\"Coin\"> {current_price.ToString()} <color=#7CFC00>{item.potionName}</color>";
                textPotion.text = $"<sprite name=\"Coin\"> {item.potionEff}";
            }
            else if (item.Type == Type_Potion.Mana)
            {
                text.text = $"<sprite name=\"Coin\"> {current_price.ToString()} <color=#76D7C4>{item.potionName}</color>";
                textPotion.text = $"<sprite name=\"Coin\"> {item.potionEff}";
            }
        }
        This_Item = Instantiate(item.gamePrefab, transform);
        sizeItem = item.gamePrefab.transform.localScale;
        This_Item.transform.localScale = sizeItem * 2f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
