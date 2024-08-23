using System.Data;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class DropItem : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject Hand_player;
    public WeaponSlot weaponSlot;

    [Header("UI Item")]
    public GameObject UI_Getitem;
    public TextMeshProUGUI text;

    [Header("UI Description")]
    public GameObject Description_pannel;
    public GameObject Pannel;
    public TextMeshProUGUI textDmg;
    public TextMeshProUGUI textCost;
    public TextMeshProUGUI textAtkSpeed;

    [Header("Weapon Data")]
    public string weaponName;
    public float Dmg;
    public int Cost;
    public float AtkSpeed;
    public Type type_weapon;
    public Rarity raritys;
    void Start()
    {
        SetStatus();
        GetButton = transform.Find("Get_Button_Item").gameObject;
        UI_Getitem = transform.Find("UI_GetItem").gameObject;
        Description_pannel = transform.Find("Description_Weapon").gameObject;
        Pannel = Description_pannel.transform.Find("Pannel").gameObject;
        textDmg = Pannel.transform.Find("textDmg_Weapon")?.GetComponentInChildren<TextMeshProUGUI>();
        textCost = Pannel.transform.Find("textCost_Weapon")?.GetComponentInChildren<TextMeshProUGUI>();
        textAtkSpeed = Pannel.transform.Find("textAtkSpeed_Weapon")?.GetComponentInChildren<TextMeshProUGUI>();
        text = UI_Getitem.GetComponentInChildren<TextMeshProUGUI>();
        GetButton.SetActive(false);
        if (gameObject.name == "Pistol(Clone)")
        {
            this.enabled = false;
            UI_Getitem.SetActive(false);
        }
        playerLayer = LayerMask.GetMask("Player");
        Hand_player = GameObject.Find("Handle_Item");
        Description_pannel.SetActive(false);
        weaponSlot = FindAnyObjectByType<WeaponSlot>();
    }

    private void SetStatus()
    {
        switch (type_weapon)
        {
            case Type.Throw:
                //รอ GetComponent
                break;
            case Type.Sword:
                //รอ GetComponent
                break;
            case Type.Single:
                //รอ GetComponent
                break;
            case Type.Swing:
                //รอ GetComponent
                break;
            case Type.FullAuto:
                //รอ GetComponent
                break;
            case Type.Magic:
                //รอ GetComponent
                break;
            default:
                //รอ GetComponent
                break;
        }
    }

    void Update()
    {
            IsPlayerInRange();
    }
    public void UpdateRarity()
    {
        switch (raritys)
        {
            case Rarity.Common:
                text.text = $"{weaponName}";
                break;
            case Rarity.Uncommon:
                text.text = $"<color=#7CFC00>{weaponName}</color>";
                break;
            case Rarity.Rare:
                text.text = $"<color=#48C9B0>{weaponName}</color>";
                break;
            case Rarity.Epic:
                text.text = $"<color=#BA55D3>{weaponName}</color>";
                break;
            case Rarity.Legendary:
                text.text = $"<color=#F7DC6F>{weaponName}</color>";
                break;
            default:
                text.text = "Unknow";
                break;
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
                Description_pannel.SetActive(true);
                UpdateDescription();
                UpdateRarity();
                return true;
            }
        }
        GetButton.SetActive(false);
        UI_Getitem.SetActive(false);
        Description_pannel.SetActive(false);
        return false;
    }

    private void UpdateDescription()
    {
        textDmg.text = $"<sprite name=\"Coin\"> {Dmg}";
        textCost.text = $"<sprite name=\"Coin\"> {Cost}";
        textAtkSpeed.text = $"<sprite name=\"Coin\"> {AtkSpeed}";
    }

    public void MoveItemToHand()
    {
            UI_Getitem.SetActive(false);
            Description_pannel.SetActive(false);
            GetButton.SetActive(false);
            weaponSlot.AddWeapon(gameObject);
            gameObject.transform.SetParent(Hand_player.transform);
            gameObject.transform.localPosition = Vector3.zero;
            this.enabled = false;
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
