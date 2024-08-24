using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int Health;
    public int Armor;
    public int Mana;

    [Header("Start Stat")]
    public int MaxHealth;
    public int MaxArmor;
    public int MaxMana;

    [Header("UI Stat Bar")]
    public Image HpBar;
    public Image ArmorBar;
    public Image ManaBar;

    [Header("Text Stat")]
    public TextMeshProUGUI textHp;
    public TextMeshProUGUI textArmor;
    public TextMeshProUGUI textMana;

    [Header("Armor Regeneration")]
    public int ArmorRegenAmount = 1;
    public float TimeToRegen = 5f;
    public float RegenInterval = 1f;
    private float lastDamageTime;
    private Coroutine regenCoroutine;

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
    private void Start()
    {
        InitializeStats();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamgeAll(1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseMana(10);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            RecoverMana(10);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Heal(1);
        }

        if (Time.time - lastDamageTime >= TimeToRegen && regenCoroutine == null)
        {
            regenCoroutine = StartCoroutine(RegenerateArmor());
        }
    }
    void UpdateUIHp()
    {
        textHp.text = $"{Health}/{MaxHealth}";
    }
    void UpdateUIMana()
    {
        textMana.text = $"{Mana}/{MaxMana}";
    }
    void UpdateUIArmor()
    {
        textArmor.text = $"{Armor}/{MaxArmor}";
    }

    private void InitializeStats()
    {
        Health = MaxHealth;
        Armor = MaxArmor;
        Mana = MaxMana;
    }
    public void TakeDamgeAll(int damgae)
    {
        if(Armor > 0)
        {
            TakeArmorDamage(damgae);
        }
        else if(Armor <= 0)
        {
            TakeDamageHp(damgae);
        }
    }
    public void TakeDamageHp(int damage)
    {
        lastDamageTime = Time.time;
        StopArmorRegen();
        Health -= damage;
        Health = Mathf.Max(Health, 0);
        HpBar.fillAmount = (float)Health / MaxHealth;
        UpdateUIHp();

        if (Health <= 0)
        {
            Die();
        }
    }

    public void TakeArmorDamage(int damage)
    {
        lastDamageTime = Time.time;
        StopArmorRegen();
        Armor -= damage;
        Armor = Mathf.Max(Armor, 0);
        ArmorBar.fillAmount = (float)Armor / MaxArmor;
        UpdateUIArmor();
    }

    public void UseMana(int amount)
    {
        Mana -= amount;
        Mana = Mathf.Max(Mana, 0);
        ManaBar.fillAmount = (float)Mana / MaxMana;
        UpdateUIMana();
    }

    public void RecoverMana(int amount)
    {
        Mana += amount;
        Mana = Mathf.Min(Mana, MaxMana);
        ManaBar.fillAmount = (float)Mana / MaxMana;
        UpdateUIMana();
    }

    public void Heal(int amount)
    {
        Health += amount;
        Health = Mathf.Min(Health, MaxHealth);
        HpBar.fillAmount = (float)Health / MaxHealth;
        UpdateUIHp();
    }

    private IEnumerator RegenerateArmor()
    {
        while (Armor < MaxArmor)
        {
            Armor += ArmorRegenAmount;
            Armor = Mathf.Min(Armor, MaxArmor);
            ArmorBar.fillAmount = (float)Armor / MaxArmor;
            UpdateUIArmor();
            yield return new WaitForSeconds(RegenInterval);
        }
        regenCoroutine = null;
    }

    private void StopArmorRegen()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    private void Die()
    {
        Debug.Log("Die");
    }
}
