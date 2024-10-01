using UnityEngine;

public class Skill_Room : MonoBehaviour
{
    [Header("Skill System")]
    public GameObject SkillRoomPrefab;
    void Start()
    {
        Instantiate(SkillRoomPrefab, transform);
    }
}
