using UnityEngine;
using System.Collections;

public class EShooterScript : Enemy
{
    [Header("Shooter Settings")]
    [SerializeField] private float shootCooldown = 5f; // Time between shots
    [SerializeField] private float shootInterval = 0.3f; 
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab to shoot
    [SerializeField] private Transform bulletSpawnPoint; // Where the bullet spawns
    private float shootTimer = 0f;

    private bool shootOrder = true; // if true will shoot up if not will shoot down

    void Update()
    {
        if (!hasFreeWill) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            StartCoroutine(Shoot(shootOrder));
            shootOrder = !shootOrder;
            shootTimer = shootCooldown + shootInterval * 9;
        }
    }

    private IEnumerator Shoot(bool order)
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position + new Vector3(-1, 0, 0), Quaternion.identity);
            bullet.transform.localScale = transform.localScale; // Match the scale of the enemy
            EBulletScript bulletScript = bullet.GetComponent<EBulletScript>();
            if (bulletScript != null)
            {
                bulletScript.direction = new Vector3(-1, order ? -0.4f + 0.1f * i : 0.4f - 0.1f * i, 0).normalized;
            }
            AudioManager_UFO.Instance.PlayEnemy(AudioType_UFO.shooterShootEnemy);
            yield return new WaitForSeconds(shootInterval); // Delay between shots
        }

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.left * 2f;
        float elapsed = 0f;
        while (elapsed < shootCooldown)
        {
            float t = elapsed / shootCooldown;
            // EaseInOutQuad
            if (t < 0.5f)
                t = 2f * t * t;
            else
                t = -1f + (4f - 2f * t) * t;

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        
        
    }
}
