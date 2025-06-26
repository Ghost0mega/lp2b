using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int health;
    protected bool hasFreeWill = false; // If false, the enemy will not do its update() logic
    [Header("Death Animation Settings")]
    protected bool isMajorEnemy = false; //Used only for death animation
    [SerializeField] private GameObject MiniExplosionPrefab; // Optional explosion effect on death
    [SerializeField] private GameObject ExplosionPrefab; // Optional explosion effect on death for major enemies



    protected virtual void Start()
    {
        if (health <= 0)
        {
            health = 1; // Ensure health is at least 1 to prevent immediate death
        }
        transform.position = new Vector3(11, Random.Range(-3f, 3f), 0f);
        if (MiniExplosionPrefab == null || ExplosionPrefab == null)
        {
            Debug.LogWarning("MiniExplosionPrefab is not assigned in the Enemy script.");
        }
        StartCoroutine(MoveOntoScreen(8));
    }

    private IEnumerator MoveOntoScreen(int finalX)
    {
        // Debug.Log("Moving enemy onto screen...");
        Vector3 targetPosition = new Vector3(finalX, transform.position.y, 0f);
        Vector3 velocity = Vector3.zero;
        float smoothTime = 0.6f; // Lower = faster, higher = slower/easier out

        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            yield return null;
        }
        // transform.position = targetPosition; // No need to snap "it just works" -Todd Howard
        yield return new WaitForSeconds(0.3f);
        hasFreeWill = true;
    }
    
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage: " + damage + ", remaining health: " + health);
        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Play damage animation or sound if needed
        }
    }
    protected void Die()
    {
        if (isMajorEnemy)
        {
            // Play major enemy death animation
            // Example: GetComponent<Animator>().SetTrigger("Die");
        }
        else
        {
            GameObject newAnim = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            newAnim.transform.localScale = transform.localScale * 0.5f;
        }
        Destroy(gameObject);
    }
}
