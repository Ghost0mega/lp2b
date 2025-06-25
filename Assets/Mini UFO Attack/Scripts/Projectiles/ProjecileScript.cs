using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Projecile : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public bool isAllowedMovement = true;
    public float lifetime = 8f; // Time before the projectile is destroyed
    public int damage = 0;

    public bool isPlayerProjectile = false; // Indicates if the projectile belongs to the player
    private float timer;
    public virtual void Launch(Vector3 direction) { /*criket*/ }

    private void Start()
    {
        timer = lifetime; // Initialize the timer with the lifetime value
    }

    private void Update()
    {
        if (isAllowedMovement)
        {
            Mouvement();
        }

        DeathChecker();
    }

    private void Mouvement()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void DeathChecker()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }

        if (transform.position.y > 6f || transform.position.y < -6f ||
            transform.position.x > 10f || transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerProjectile)
        {
            PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the projectile on collision with the player
        }

        else if (collision.gameObject.CompareTag("Enemy") && isPlayerProjectile)
        {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the projectile on collision with the enemy
        }
    }
}
