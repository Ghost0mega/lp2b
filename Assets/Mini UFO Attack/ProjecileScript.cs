using UnityEngine;

public class Projecile : MonoBehaviour
{
    public float speed;
    public float lifetime = 5f; // Time before the projectile is destroyed
    private float timer;
    public virtual void Launch(Vector3 direction) { /*criket*/ }
    private void Start()
    {
        timer = lifetime; // Initialize the timer with the lifetime value
    }
    private void Update()
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
}
