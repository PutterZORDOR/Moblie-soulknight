using Unity.Android.Gradle.Manifest;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Item", menuName = "Scriptable Objects/Weapon_Item")]
public class Weapon_Item : ScriptableObject
{
    public string itemName;
    public float Damgae;
    public int Mana;
    public Rarity rarity;

    [Header("Prefab")]
    public GameObject gamePrefab;
}
public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }