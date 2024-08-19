﻿using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Range;
    [SerializeField] private bool Detected = false;
    private Vector3 Direction;
    public GameObject weapon;
    public float rotationSpeed = 5f;

    public Transform ShootPoint;
    public float Force;

    [SerializeField] private bool flipped = false;
    public Transform characterTransform;

    public JoystickMove joystickMoveScript;

    void Update()
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

            if ( Mathf.Abs(currentAngle) <= 90 && !flipped)
            {
                flipped = true;
                FlipCharacter();
            }
            else if (Mathf.Abs(currentAngle) > 90 && flipped)
            {
                flipped = false;
                UnflipCharacter();
            }
        }
    }

    private void FlipCharacter()
    {
        if (characterTransform != null)
        {
            Vector3 scale = characterTransform.localScale;
            // Flip to the left by setting x to -1 if it is currently 1
            if (scale.x > 0)
            {
                scale.x = -1;
                characterTransform.localScale = scale;
            }
        }
    }

    private void UnflipCharacter()
    {
        if (characterTransform != null)
        {
            Vector3 scale = characterTransform.localScale;
            // Unflip back to the right by setting x to 1 if it is currently -1
            if (scale.x < 0)
            {
                scale.x = 1;
                characterTransform.localScale = scale;
            }
        }
    }

    private void CorrectCharacterFlip()
    {
        // Ensure the character is facing the right direction based on its movement.
        joystickMoveScript.FlipCharacterBasedOnDirection();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(joystickMoveScript.transform.position, Range);
    }
}
