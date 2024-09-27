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

    [Header("Invulnerability")]
    public float invulnerabilityDuration;  
    private bool isInvulnerable = false;

    [Header("UI GameOver")]
    public GameObject UI_GameOver;

    [Header("Player TakeDamge")]
    private SpriteRenderer spriteRenderer;
    public float blinkTime;
    public Color blinkColor;
    public Color ghostColor;
    private GameObject player;
    public JoystickMove joy;

    [Header("Bleed delay")]
    public float delayBetweenDamage;
    public bool decreaseBleeding;

    [Header("Boost Damge")]
    public int damgeMulti;

    [Header("List My Skill")]
    public Sprite[] skills = new Sprite[2];

    [Header("Skill Player")]
    public Image skill_1;
    public Image skill_2;

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
        player = GameObject.FindGameObjectWithTag("Player");
        joy = player.GetComponent<JoystickMove>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
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

    private void InitializeStats()
    {
        Health = MaxHealth;
        Armor = MaxArmor;
        Mana = MaxMana;
    }

    public void TakeDamgeAll(int damage)
    {
        if (isInvulnerable) return;

        if (!joy.isDashing)
        {
            if (Armor > 0)
            {
                TakeArmorDamage(damage);
            }
            else
            {
                TakeDamageHp(damage);
            }

            StartCoroutine(InvulnerabilityTimer());
        }
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        spriteRenderer.color = blinkColor;
        yield return new WaitForSeconds(blinkTime);
        spriteRenderer.color = ghostColor;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
        spriteRenderer.color = Color.white;
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
    public void DecreaseMaxHealth(int amount)
    {
        MaxHealth -= amount;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }

        HpBar.fillAmount = (float)Health / MaxHealth;
        UpdateUIHp();
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
        Time.timeScale = 0f;
        UI_GameOver.SetActive(true);
        GameOverManager.instance.UpdateText();
    }
    private Coroutine bleedCoroutine;

    public void StartBleeding(int totalDamage)
    {
        if (bleedCoroutine != null)
        {
            StopCoroutine(bleedCoroutine);
        }

        bleedCoroutine = StartCoroutine(Bleed(totalDamage));
    }

    private IEnumerator Bleed(float totalDamage)
    {
        float remainingDamage = totalDamage;
        if (decreaseBleeding)
        {
            remainingDamage = remainingDamage * 0.5f;
        }

        while (remainingDamage > 0)
        {
            TakeDamgeAll(1);
            remainingDamage--;

            yield return new WaitForSeconds(delayBetweenDamage);
        }

        bleedCoroutine = null;
    }
    public void IncreaseMaxHealth(int amount)
    {
        MaxHealth += amount;
        Health = Mathf.Min(Health, MaxHealth);
        HpBar.fillAmount = (float)Health / MaxHealth;
        UpdateUIHp();
    }
    public void IncreaseMaxArmor(int amount)
    {
        MaxArmor += amount;
        Armor = Mathf.Min(Armor, MaxArmor);
        ArmorBar.fillAmount = (float)Armor / MaxArmor;
        UpdateUIArmor();
    }


    private void UpdateUIHp()
    {
        textHp.text = $"{Health}/{MaxHealth}";
    }

    private void UpdateUIArmor()
    {
        textArmor.text = $"{Armor}/{MaxArmor}";
    }

    private void UpdateUIMana()
    {
        textMana.text = $"{Mana}/{MaxMana}";
    }
    public void AddSkill(Sprite newSkill)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = newSkill;
                return;
            }
        }
    }
}
