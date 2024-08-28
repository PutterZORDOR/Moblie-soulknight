using System;
using UnityEngine;

[CreateAssetMenu(fileName = "All_Potion", menuName = "Scriptable Objects/All_Potion")]
public class All_Potion : ScriptableObject
{
    public string potionName;
    public Type_Potion Type;
    public int potionEff;

    [Header("Game Prefab")]
    public GameObject gamePrefab;
}
public enum Type_Potion { Heal,Mana }