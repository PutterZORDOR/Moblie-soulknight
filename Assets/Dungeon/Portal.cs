using UnityEngine;

public class Portal : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public GameObject TpButton;
    DungeonMaker dungeon;
    private void Start()
    {
        dungeon = FindAnyObjectByType<DungeonMaker>();
        playerLayer = LayerMask.GetMask("Player");
        TpButton = transform.Find("Tp_Button").gameObject;
        TpButton.SetActive(false);
    }
    private void Update()
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
                TpButton.SetActive(true);
                return true;
            }
        }
        TpButton.SetActive(false);
        return false;
    }
    public void ChangeFloor()
    {
        dungeon.DestroyDungeon();
    }
}
