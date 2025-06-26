using UnityEngine;

public class ERammerScript : Enemy
{
    [Header("Rammer Settings")]
    [SerializeField] private float maxAcceleration = 5f;
    [SerializeField] private float maxTurningSpeed = 200f;
    [SerializeField] private Transform playerTransform;
    private Rigidbody2D rb;
    override protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        base.Start();
        health = 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasFreeWill) return;
        MoveTowardsPlayer();
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
        
}
