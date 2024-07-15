using UnityEngine;

public class Enermy : MonoBehaviour
{
    private void OnMouseDown()
    {
        DungeonSystem.instance.AllEnermyInRoom--;
        Destroy(this.gameObject);
    }
}
