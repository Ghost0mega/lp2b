using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Projecile : MonoBehaviour
{
    [Header("General Settings")]
    public float speed;
    public Vector3 direction;
    public bool isAllowedMovement = true;
    public float lifetime = 8f;
    public int damage = 0;

    public bool isPlayerProjectile = false;
    private float timer;
    
    public virtual void ContactDestroy()
    {
        // This method can be overridden in derived classes to handle specific destruction logic
        Destroy(gameObject);
    }

    protected virtual void Start()
    {
        timer = lifetime; // Initialize the timer with the lifetime value
    }

    protected virtual void Update()
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
            ContactDestroy();
        }  

        if (transform.position.y > 6f || transform.position.y < -6f ||
            transform.position.x > 10f || transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerProjectile)
        {
            PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
            ContactDestroy(); 
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
            ContactDestroy();
        }
    }
}
