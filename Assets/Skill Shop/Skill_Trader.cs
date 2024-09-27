using TMPro;
using UnityEngine;

public class Skill_Trader : MonoBehaviour
{
    public WeightSkill lootTable;

    [Header("Price")]
    public int Price_Life;

    public bool CanBuy = true;
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject This_Item;
    public GameObject UI_Buy;
    private Vector3 sizeItem;
    [SerializeField] int current_price;
    [SerializeField] All_Skill item;

    [Header("UI")]
    public TextMeshProUGUI text;

    [Header("UI Description")]
    public GameObject Description;
    public TextMeshProUGUI textSkill;
    void Start()
    {
        item = lootTable.GetRandom();
        playerLayer = LayerMask.GetMask("Player");
        GetButton = transform.Find("Get_Button").gameObject;
        Description = transform.Find("Description_Skill").gameObject;
        textSkill = Description.GetComponentInChildren<TextMeshProUGUI>();
        UI_Buy = transform.Find("UI_Buy").gameObject;
        text = UI_Buy.GetComponentInChildren<TextMeshProUGUI>();
        UI_Buy.SetActive(false);
        Description.SetActive(false);
        GetButton.SetActive(false);
        ShowItem();
    }
    void Update()
    {
        if (CanBuy)
        {
            IsPlayerInRange();
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
    public void Buy()
    {
        if (CanBuy && PlayerManager.instance.MaxHealth > current_price)
        {
            PlayerManager.instance.DecreaseMaxHealth(current_price);
            CanBuy = false;
            UI_Buy.SetActive(false);
            Description.SetActive(false);
            GetButton.SetActive(false);
            if (item != null)
            {
                if (item.Type == Type_Skill.BoostDmg)
                {
                    PlayerManager.instance.damgeMulti = 2;
                }
                else if (item.Type == Type_Skill.DecreaseDebuff)
                {
                    PlayerManager.instance.decreaseBleeding = true;
                }
                else if (item.Type == Type_Skill.IncreaseSpeedAndDash)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    JoystickMove joy = player.GetComponent<JoystickMove>();
                    joy.IncreaseSpeed(2f);
                }
                else if (item.Type == Type_Skill.IncreaseArmor)
                {
                    PlayerManager.instance.IncreaseMaxArmor(3);
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
    void ShowItem()
    {
        if (item != null)
        {
            if (item.Type == Type_Skill.DecreaseDebuff)
            {
                current_price = Price_Life + 1;
            }
            else
            {
                current_price = Price_Life;
            }

            text.text = $"<sprite name=\"Coin\"> {current_price.ToString()} <color=#7CFC00>{item.skillname}</color>";
            textSkill.text = $"{item.Description}";
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
