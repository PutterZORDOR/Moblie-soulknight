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

    protected virtual void Awake()
    {
        // Initialize references
        joystickMoveScript = FindObjectOfType<JoystickMove>();
        characterTransform = transform;

        // Automatically find weapon and AttackPoint
        InitializeWeapon();
    }

    public virtual void InitializeWeapon()
    {
        // Default implementation, can be overridden in derived classes
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
        if (weapon.gameObject.layer == LayerMask.NameToLayer("Gun"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                Detected = false;
                joystickMoveScript.EnableFlip();
                CorrectCharacterFlip();  // Ensure the character faces the right direction when no enemy is detected
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
                joystickMoveScript.DisableFlip();
            }
            else
            {
                Detected = false;
                joystickMoveScript.EnableFlip();
                CorrectCharacterFlip();  // Ensure correct orientation when leaving enemy range
            }

            if (Detected)
            {
                // Calculate direction from weapon to enemy
                Vector3 direction = closestEnemy.transform.position - weapon.transform.position;
                // Calculate the angle for rotation
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Ensure that weapon rotates smoothly
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Update angle for character flip
                float currentAngle = weapon.transform.eulerAngles.z;
                if (currentAngle > 180) currentAngle -= 360;

                if (Mathf.Abs(currentAngle) > 90 && !flipped)
                {
                    flipped = true;
                    FlipCharacter();
                }
                else if (Mathf.Abs(currentAngle) <= 90 && flipped)
                {
                    flipped = false;
                    UnflipCharacter();
                }
            }
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

    protected abstract void Attack();
    private void FlipCharacter()
    {
        if (characterTransform != null)
        {
            // หยุดอนิเมชั่นชั่วคราว
            Animator animator = characterTransform.GetComponent<Animator>();
            if (animator != null) animator.enabled = false;

            Vector3 scale = characterTransform.localScale;
            if (scale.x < 0)
            {
                scale.x = 1;
                characterTransform.localScale = scale;
                weapon.transform.localScale = scale;
            }

            // เปิดใช้งานอนิเมชั่นอีกครั้ง
            if (animator != null) animator.enabled = true;
        }
    }

    private void UnflipCharacter()
    {
        if (characterTransform != null)
        {
            Animator animator = characterTransform.GetComponent<Animator>();
            if (animator != null) animator.enabled = false;

            Vector3 scale = characterTransform.localScale;
            if (scale.x > 0)
            {
                scale.x = -1;
                characterTransform.localScale = scale;
                weapon.transform.localScale = scale;
            }

            if (animator != null) animator.enabled = true;
        }
    }

    private void CorrectCharacterFlip()
    {
        joystickMoveScript.FlipCharacterBasedOnDirection();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(joystickMoveScript.transform.position, Range);
    }
}
