using UnityEngine;

public class ERammerScript : Enemy
{
    [Header("Rammer Settings")]
    [SerializeField] private float maxAcceleration = 5f;
    [SerializeField] private float maxTurningSpeed = 200f;
    [SerializeField] private Transform playerTransform;
    public int damage = 1;
    private Rigidbody2D rb;
    override protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        base.Start();
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasFreeWill) return;
        MoveTowardsPlayer();
        if (transform.position.x < -15f)
        {
            Die();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform is not assigned in ERammerScript.");
            return;
        }

        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D is missing on ERammerScript GameObject.");
            return;
        }

        Vector2 toPlayer = (playerTransform.position - transform.position).normalized;

        float angleToPlayer = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        float angleDiff = Mathf.DeltaAngle(rb.rotation, angleToPlayer);

        float rotationStep = maxTurningSpeed * Time.deltaTime;
        float rotation = Mathf.Clamp(angleDiff, -rotationStep, rotationStep);
        rb.MoveRotation(rb.rotation + rotation);

        // Accelerate forward
        Vector2 forward = -rb.transform.right;
        rb.AddForce(forward * maxAcceleration, ForceMode2D.Force);
        // why no turn? :(
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TakeDamage(damage);
                // Debug.Log("Rammer collided with player, dealing " + damage + " damage.");
                health = 0;
                Die();
            }
            else
            {
                Debug.LogWarning("PlayerScript component not found on player GameObject.");
            }
            
        }
    }
}
