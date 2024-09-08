using Unity.VisualScripting;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public GameObject[] weapons = new GameObject[2];
    public GameObject First_item;
    private int currentWeaponIndex = 0;

    public Vector3 dropOffset = new Vector3(0, 0, 1);

    void Start()
    {
        GameObject Hand_player = GameObject.Find("Handle_Item");
        GameObject item = Instantiate(First_item, Hand_player.transform);
        item.transform.SetParent(Hand_player.transform);
        item.transform.localPosition = Vector3.zero;
        Vector3 size = item.transform.localScale;
        item.transform.localScale = size * 2;
        weapons[currentWeaponIndex] = item;

        foreach (GameObject weapon in weapons)
        {
            if (weapon != null)
            {
                weapon.SetActive(false);
                weapon.tag = "UnEquipped"; // Set tag to UnEquipped for all initial weapons
            }
        }

        if (weapons[0] != null)
        {
            weapons[0].SetActive(true);
            SetWeaponSlotStatus(weapons[0], true);
            weapons[0].tag = "Equipped"; // Set tag to Equipped for the first weapon
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        int nextWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;

        if (weapons[nextWeaponIndex] != null)
        {
            SetWeaponSlotStatus(weapons[currentWeaponIndex], false); // Remove status from current weapon
            weapons[currentWeaponIndex]?.SetActive(false);
            weapons[currentWeaponIndex].tag = "UnEquipped"; // Change tag to UnEquipped for current weapon

            currentWeaponIndex = nextWeaponIndex;

            weapons[currentWeaponIndex]?.SetActive(true);
            SetWeaponSlotStatus(weapons[currentWeaponIndex], true); // Add status to new weapon
            weapons[currentWeaponIndex].tag = "Equipped"; // Change tag to Equipped for new weapon
        }
    }

    void DropWeapon()
    {
        if (weapons[currentWeaponIndex] != null)
        {
            DropItem drop = weapons[currentWeaponIndex].GetComponent<DropItem>();
            GameObject currentWeapon = weapons[currentWeaponIndex];

            SetWeaponSlotStatus(currentWeapon, false); // Unset isInWeaponSlot before dropping
            currentWeapon.tag = "UnEquipped"; // Set tag to UnEquipped when dropped

            currentWeapon.transform.SetParent(null);
            currentWeapon.transform.position = transform.position + dropOffset;
            drop.enabled = true;
            weapons[currentWeaponIndex] = null;
        }
    }

    public void AddWeapon(GameObject newWeapon)
    {
        if (weapons[currentWeaponIndex] != null)
        {
            SetWeaponSlotStatus(weapons[currentWeaponIndex], false); // Unset isInWeaponSlot
            weapons[currentWeaponIndex].SetActive(false);
            weapons[currentWeaponIndex].tag = "UnEquipped"; // Set current weapon to UnEquipped
        }

        bool weaponAdded = false;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                weapons[i] = newWeapon;
                currentWeaponIndex = i;
                weapons[currentWeaponIndex].SetActive(true);
                SetWeaponSlotStatus(weapons[currentWeaponIndex], true); // Set isInWeaponSlot
                weapons[currentWeaponIndex].tag = "Equipped"; // Set new weapon to Equipped

                weaponAdded = true;
                break;
            }
        }

        if (!weaponAdded)
        {
            DropWeapon();
            weapons[currentWeaponIndex] = newWeapon;
            weapons[currentWeaponIndex].SetActive(true);
            SetWeaponSlotStatus(weapons[currentWeaponIndex], true); // Set isInWeaponSlot
            weapons[currentWeaponIndex].tag = "Equipped"; // Set new weapon to Equipped
        }
    }

    private void SetWeaponSlotStatus(GameObject weapon, bool isInSlot)
    {
        Weapon weaponComponent = weapon.GetComponent<Weapon>();
        if (weaponComponent != null)
        {
            weaponComponent.isInWeaponSlot = isInSlot;
        }
    }
}
