using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Item", menuName = "Scriptable Objects/Weapon_Item")]
public class Weapon_Item : ScriptableObject
{
    public string itemName;
    public float Damgae;
    public int Mana;
    public float AtkSpeed;
    public Rarity rarity;
    public Type weaponType;

    [Header("Prefab")]
    public GameObject gamePrefab;
}
public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }
public enum Type { Sword, Swing, Hammer, Magic, Throw, Shotgun, Single, FullAuto}