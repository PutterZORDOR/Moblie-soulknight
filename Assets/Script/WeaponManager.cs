using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Gun[] guns; // Ensure these are of type Weapon or derived types
    private int currentGunIndex = 0;

    public Transform weaponTransform; // This will be assigned based on the active weapon

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipGun(0); // ลูกซอง
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipGun(1); // ปืนพก
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipGun(2); // ปืนกล
    }

    void EquipGun(int index)
    {
        currentGunIndex = index;
        weaponTransform = guns[currentGunIndex].transform; // Update weaponTransform

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].gameObject.SetActive(i == currentGunIndex);
        }
        Debug.Log("Equipped " + guns[currentGunIndex].name);
    }
}
