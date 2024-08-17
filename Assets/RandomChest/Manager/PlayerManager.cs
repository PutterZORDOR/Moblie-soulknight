using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int Health;
    public int MaxHealth;
    public int Mana;
    public int MaxMana;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitializeStats()
    {
        MaxHealth = 5;
        Health = MaxHealth;
        MaxMana = 200;
        Mana = MaxMana;
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        Health = Mathf.Max(Health, 0); // ตรวจสอบให้มั่นใจว่า HP ไม่ต่ำกว่า 0

        if (Health <= 0)
        {
            Die();
        }
    }
    public void RecoverMana(int amount)
    {
        Mana += amount;
        Mana = Mathf.Min(Mana, MaxMana); // ตรวจสอบให้มั่นใจว่า Mana ไม่เกิน MaxMana
    }
    public void Heal(int amount)
    {
        Health += amount;
        Health = Mathf.Min(Health, MaxHealth); // ตรวจสอบให้มั่นใจว่า HP ไม่เกิน MaxHealth
    }

    private void Die()
    {
        Debug.Log("Die");
    }


}
