using UnityEngine;

public class BrickScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
            return;
        }

        // Set the color of the brick to a random color
        spriteRenderer.color = Color.HSVToRGB(Random.Range(0f, 1f), .3f, 1f);

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the ball
        if (collision.gameObject.CompareTag("Ball"))
        {
            Destroy(gameObject);
        }
    }
}
