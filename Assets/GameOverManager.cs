using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI SumCoin_Text;
    public TextMeshProUGUI FloorClear_Text;
    public static GameOverManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateText()
    {
        SumCoin_Text.text = $"<sprite name=\"Coin\"> {CoinManager.instance.SumCoin}";
        FloorClear_Text.text = $"<sprite name=\"Coin\"> {DungeonSystem.instance.Level}";
    }
    public void LoadFirstScene()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0);
    }
}
