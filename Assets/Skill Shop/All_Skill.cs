using UnityEngine;

[CreateAssetMenu(fileName = "All_Skill", menuName = "Scriptable Objects/All_Skill")]
public class All_Skill : ScriptableObject
{
    public string skillname;
    public Type_Skill Type;

    [TextArea(3, 10)]
    public string Description;

    [Header("Game Prefab")]
    public GameObject gamePrefab;

    [Header("Sprite")]
    public Sprite sprite;

}
public enum Type_Skill { BoostDmg, DecreaseDebuff, IncreaseArmor, IncreaseSpeedAndDash}
