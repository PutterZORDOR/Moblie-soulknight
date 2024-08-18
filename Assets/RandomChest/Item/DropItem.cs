using UnityEngine;

public class DropItem : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject GetButton;
    public GameObject Hand_player;
    public WeaponSlot weaponSlot;
    public GameObject UI_Getitem;
    void Start()
    {
        GetButton = transform.Find("Get_Button_Item").gameObject;
        UI_Getitem = transform.Find("UI_GetItem").gameObject;
        UI_Getitem.SetActive(false);
        GetButton.SetActive(false);
        if (gameObject.name == "Pistol(Clone)")
        {
            this.enabled = false;
        }
        playerLayer = LayerMask.GetMask("Player");
        Hand_player = GameObject.Find("Handle_Item");
        weaponSlot = FindAnyObjectByType<WeaponSlot>();
    }
    void Update()
    {
            IsPlayerInRange();
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
        GetButton.SetActive(false);
        UI_Getitem.SetActive(false);
        return false;
    }
    public void MoveItemToHand()
    {
            UI_Getitem.SetActive(false);
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
