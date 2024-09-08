using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    public int Damage = 10;
    public float Range;

    [SerializeField] private bool Detected = false;
    private Vector3 Direction;
    public GameObject weapon;
    public float rotationSpeed = 5f;

    [SerializeField] public Transform AttackPoint;
    [SerializeField] public float Force;

    [SerializeField] private bool flipped = false;
    public Transform characterTransform;
    public JoystickMove joystickMoveScript;

    public InputAction attackAction;
    public float attackRate = 0.2f;
    public float nextAttackTime = 0f;

    // Flip cooldown variables
    public float flipCooldown = 0.5f;  // Time between flips in seconds
    private float lastFlipTime = 0f;    // Tracks when the last flip happened

    private SpriteRenderer spriteRenderer;

    public bool isInWeaponSlot = false;

    protected virtual void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        joystickMoveScript = player.GetComponent<JoystickMove>();
        AttackPoint = player.transform.Find("Attack_Point");
        characterTransform = player.transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if(weapon.gameObject.layer == LayerMask.NameToLayer("Gun"))
        {
            joystickMoveScript.weaponTransform = weapon.transform;
            joystickMoveScript.weapon = weapon.GetComponent<Weapon>();
        }
        else
        {
            joystickMoveScript.weaponTransform = null;
            joystickMoveScript.weapon = weapon.GetComponent<Weapon>();
        }
    }
    private void OnEnable()
    {
        attackAction.Enable();
    }

    private void OnDisable()
    {
        attackAction.Disable();
    }

    private void Update()
    {
        if (isInWeaponSlot)
        {
            if (Detected)
            {
                joystickMoveScript.DisableFlip();
            }
            else
            {
                joystickMoveScript.EnableFlip();
            }

            if (weapon.gameObject.layer == LayerMask.NameToLayer("Gun"))
            {
                HandleGun();
            }
            else if (weapon.gameObject.layer == LayerMask.NameToLayer("Sword"))
            {
                HandleSword();
            }
            if (attackAction.ReadValue<float>() > 0)
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackRate;
                }
            }
        }
    }

    private void HandleGun()
    {
        if (joystickMoveScript == null)
        {
            Debug.LogError("JoystickMove script is not assigned!");
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            Detected = false;
            CorrectCharacterFlip();
            joystickMoveScript.enableRotateWeapon = true; // Ensure this line is executed
            return;
        }

        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(joystickMoveScript.transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        if (closestDistance <= Range)
        {
            Direction = closestEnemy.transform.position - (Vector3)transform.position;
            Detected = true;
        }
        else
        {
            Detected = false;
            CorrectCharacterFlip();
            joystickMoveScript.enableRotateWeapon = true; // Ensure this line is executed
        }

        if (Detected)
        {
            joystickMoveScript.enableRotateWeapon = false;
            Vector3 direction = closestEnemy.transform.position - weapon.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float currentAngle = weapon.transform.eulerAngles.z;
            if (currentAngle > 180) currentAngle -= 360;  // Normalize the angle

            // Flip only if cooldown has passed
            if (Time.time - lastFlipTime > flipCooldown)
            {
                if (Mathf.Abs(currentAngle) > 90 && !flipped)
                {
                    flipped = true;
                    FlipCharacter();
                    lastFlipTime = Time.time;  // Update last flip time
                }
                else if (Mathf.Abs(currentAngle) <= 90 && flipped)
                {
                    flipped = false;
                    UnflipCharacter();
                    lastFlipTime = Time.time;  // Update last flip time
                }
            }
        }
        else
        {
            joystickMoveScript.enableRotateWeapon = true; // Ensure this line is executed
        }
    }

    private void HandleSword()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            Detected = false;
            CorrectCharacterFlip();
            return;
        }

        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(joystickMoveScript.transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        if (closestDistance <= Range)
        {
            Direction = closestEnemy.transform.position - (Vector3)transform.position;
            Detected = true;
        }
        else
        {
            Detected = false;
            CorrectCharacterFlip();
        }

        if (Detected)
        {
            // Check cooldown before flipping
            if (Time.time - lastFlipTime > flipCooldown)
            {
                // Flip based on the enemy's position relative to the player
                if (Direction.x > 0 && flipped)
                {
                    flipped = false;
                    UnflipCharacterS();
                    lastFlipTime = Time.time;  // Update last flip time
                }
                else if (Direction.x < 0 && !flipped)
                {
                    flipped = true;
                    FlipCharacterS();
                    lastFlipTime = Time.time;  // Update last flip time
                }
            }
        }
    }

    private void UnflipCharacterS()
    {
        if (characterTransform != null)
        {
            Vector3 scale = characterTransform.localScale;
            if (scale.x > 0)
            {
                scale.x = -2;
                characterTransform.localScale = scale;
            }
        }
    }

    private void FlipCharacterS()
    {
        if (characterTransform != null)
        {
            Vector3 scale = characterTransform.localScale;
            if (scale.x < 0)
            {
                scale.x = 2;
                characterTransform.localScale = scale;
            }
        }
    }

    protected abstract void Attack();

    private void FlipCharacter()
    {
        if (characterTransform != null)
        {
            Vector3 scale = characterTransform.localScale;
            if (scale.x < 0)
            {
                scale.x = 2;
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = true;
                characterTransform.localScale = scale;
            }
        }
    }

    private void UnflipCharacter()
    {
        if (characterTransform != null)
        {
            Vector3 scale = characterTransform.localScale;
            if (scale.x > 0)
            {
                scale.x = -2;
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
                characterTransform.localScale = scale;
            }
        }
    }

    private void CorrectCharacterFlip()
    {
        joystickMoveScript.FlipCharacterBasedOnDirection();
    }
}
