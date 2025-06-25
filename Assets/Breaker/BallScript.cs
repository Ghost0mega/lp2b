using UnityEngine;
using System.Collections;


public class BallScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float initialSpeed = 5f;
    public float maxSpeed = 10f;
    private bool isSpawning = false;
    public PlacerScript placerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the ball.");
            return;
        }
        StartCoroutine(SpawnBall(false));

    }

    // Update is called once per frame
    void Update()
    {
        if (placerScript.lives > 0)
        {
            // Check if the ball is out of bounds (below the screen)
            if (transform.position.y < -6 && !isSpawning)
            {
                AudioManager.Instance.PlaySound(AudioType.die, AudioSourceType.game);
                StartCoroutine(SpawnBall(true));
                placerScript.score -= 50;
                placerScript.lives--;
            }

            if (!isSpawning && rb.linearVelocity.magnitude < initialSpeed)
            {
                // Ensure the ball maintains a minimum speed
                rb.linearVelocity = rb.linearVelocity.normalized * initialSpeed;
            }
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                // Clamp the speed to the maximum speed
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }


            // this causes problems with spawning the ball (fixed)
            if (!isSpawning && rb.linearVelocityY <= 0.2f && rb.linearVelocityY >= -0.2f)
            {
                // Randomly adjust the vertical velocity to prevent it from getting stuck
                rb.linearVelocityY = Random.Range(-1f, 1f);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pad"))
        {
            // Get the difference in X between ball and paddle
            float diffX = transform.position.x - collision.transform.position.x;

            // Tweak the multiplier (e.g., 3) for how much the paddle affects the ball
            rb.linearVelocity += new Vector2(diffX * 3f, 0);
            AudioManager.Instance.PlaySound(AudioType.bounce, AudioSourceType.player);
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            // Bounce off the wall
            AudioManager.Instance.PlaySound(AudioType.bouncewall, AudioSourceType.game);
        }
    }

    public IEnumerator SpawnBall(bool wait)
    {
        transform.position = new Vector2(0, -2f);
        if (wait)
        {
            isSpawning = true;
            GetComponent<SpriteRenderer>().enabled = false;
            rb.linearVelocity = Vector2.zero; // Reset velocity
            yield return new WaitForSeconds(1f);
            GetComponent<SpriteRenderer>().enabled = true; // Show the ball early to avoid suprising the player
            yield return new WaitForSeconds(1f);
        }
        rb.linearVelocity = new Vector2(0, -initialSpeed); // Set initial speed
        isSpawning = false;
    }
}
