using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject bulletPrefab;
    public int bulletLayerCount = 1; // for power-ups or upgrades
    public int bulletSideSpread = 0; //hard to implement this is for later
    public float shootCooldown = 0.5f; // can be used for power-ups or upgrades
    private float shootTimer = 0f; 


    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootSound;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;
    public int lives = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Player initialized with " + lives + " lives.");
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned in the PlayerScript.");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from the PlayerScript GameObject.");
        }
        else
        {
            _audioSource.clip = _shootSound;
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            _audioSource.volume = .5f;
            _audioSource.clip = _shootSound;
        }

    }

    

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space) && shootTimer <= 0f)
        {
            shootTimer = shootCooldown;
            if (bulletPrefab != null)
            {
                Shoot(transform);
            }
            else
            {
                Debug.LogWarning("Bullet prefab is not assigned.");
            }
        }
    }

    private void Movement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -8f)
        {
            moveDirection += Vector3.left;
        }

        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 8f)
        {
            moveDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < 4f)
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -4f)
        {
            moveDirection += Vector3.down;
        }

        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();
            transform.Translate(moveDirection * speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        lives -= damage;
        if (lives <= 0)
        {
            // Die();
        }
    }

    public void Shoot(Transform spawnpoint)
    {
        float spacing = 0.3f;
        float totalWidth = (bulletLayerCount - 1) * spacing;

        for (int i = 0; i < bulletLayerCount; i++)
        {
            float yOffset = (i * spacing) - (totalWidth / 2f);

            Vector3 spawnPos = spawnpoint.position + spawnpoint.up * yOffset;
            spawnPos -= spawnpoint.forward * 0.5f;

            GameObject newBullet = Instantiate(bulletPrefab, spawnPos, spawnpoint.rotation);
        }
        _audioSource.Play();
    }
}
