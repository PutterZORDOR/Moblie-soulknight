using UnityEngine;

public class Boss : MonoBehaviour
{
    private void OnMouseDown()
    {
        DungeonSystem.instance.AllBossStatus = false;
        Destroy(this.gameObject);
    }
}
