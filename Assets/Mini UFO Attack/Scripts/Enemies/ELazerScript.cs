using UnityEngine;
using System.Collections;

public class ELazerScript : Enemy
{
    [Header("Lazer Settings")]
    [SerializeField] private float shootCooldown = 8f;
    [SerializeField] private float shootDuration = 0.5f;
    [SerializeField] private GameObject lazerPrefab;
    private float shootTimer = 0f;

    void Update()
    {
        if (!hasFreeWill) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            StartCoroutine(Shoot());
            AudioManager_UFO.Instance.PlayEnemy(AudioType_UFO.LaserShootEnemy);
        }
    }

    private IEnumerator Shoot()
    {
        shootTimer = shootCooldown + shootDuration;
        GameObject lazer = Instantiate(lazerPrefab, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
        ELazerScriptP lazerScript = lazer.GetComponent<ELazerScriptP>();
        if (lazerScript != null)
        {
            lazerScript.direction = -transform.right; // Set the direction of the lazer
        }

        yield return new WaitForSeconds(shootDuration);

        Destroy(lazer);
        
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 2f * (Random.value > 0.5f ? 1 : -1);
        if(targetPosition.y > 4f) targetPosition.y = 4f;
        if(targetPosition.y < -4f) targetPosition.y = -4f;
        
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
