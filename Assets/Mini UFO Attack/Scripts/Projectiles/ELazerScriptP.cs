using UnityEngine;

public class ELazerScriptP : Projecile
{
    [Header("Special Settings")]
    [SerializeField] private GameObject contactEffect;

    [SerializeField] private float growTime = 0.5f;
    [SerializeField] private float hitCooldown = 0.3f;
    private float hitTimer =0f;

    protected override void Update()
    {
        base.Update();
        if (transform.localScale.x < 1f)
        {
            transform.localScale += new Vector3(1f, 1f, 0f) * (Time.deltaTime / growTime);
        }
        if (hitTimer > 0f) hitTimer -= Time.deltaTime;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitTimer > 0f) return; // Prevent multiple hits in a short time

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ELazerScriptP hit the player!");
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TakeDamage(damage);
                hitTimer = hitCooldown; // Reset hit timer
                Instantiate(contactEffect, player.transform.position, Quaternion.identity);
            }
        }
    }
}
