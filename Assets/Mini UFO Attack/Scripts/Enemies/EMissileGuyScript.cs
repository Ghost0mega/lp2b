using UnityEngine;

public class EMissileGuyScript : Enemy
{
    [Header("Missile Guy Settings")]
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private float missileLifetime = 8f;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform missileSpawnPoint;
    private float shootTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!hasFreeWill) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootMissile();
            shootTimer = shootCooldown;
        }
    }
    
    private void ShootMissile()
    {
        if (missilePrefab == null || missileSpawnPoint == null || playerTransform == null)
        {
            Debug.LogWarning("Missile settings are not properly assigned in EMissileGuyScript.");
            return;
        }

        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position + new Vector3(-1, 0, 0), Quaternion.identity);
        EMissileScript missileScript = missile.GetComponent<EMissileScript>();
        if (missileScript != null)
        {
            missileScript.playerTransform = playerTransform;
            missileScript.lifetime = missileLifetime;
        }
    }
}
