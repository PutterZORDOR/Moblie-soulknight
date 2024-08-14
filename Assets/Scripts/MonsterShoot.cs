using UnityEngine;

public class MonsterShoot : MonoBehaviour
{
    public GameObject shootParticlePrefab; // Reference to the particle system prefab
    public Transform shootPoint; // The point from which the monster shoots

    void Update()
    {
        // For demonstration purposes, let's use the spacebar to trigger shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the particle system at the shoot point
        Instantiate(shootParticlePrefab, shootPoint.position, shootPoint.rotation);
    }
}
