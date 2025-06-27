using System;
using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject bulletPrefab;
    public int bulletLayerCount = 1; // for power-ups or upgrades
    public float shootCooldown = 0.5f; // can be used for power-ups or upgrades
    private float shootTimer = 0f;

    public GameObject _controller;
    private UFOControllerScript _controllerScript;

    [Header("Audio Settings")]
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootSound;

    [SerializeField] private float speed = 10f;

    [Header("Death Settings")]
    public int lives = 1;

    [SerializeField] private GameObject miniExplosionPrefab;
    [SerializeField] private GameObject explosionPrefab;
    void Start()
    {
        if (_controller == null)
        {
            Debug.LogError("Controller is not assigned in the PlayerScript.");
            return;
        }
        _controllerScript = _controller.GetComponent<UFOControllerScript>();
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
        }
        if (miniExplosionPrefab == null || explosionPrefab == null)
        {
            Debug.LogWarning("Explosion prefabs are not assigned in the PlayerScript.");
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
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < 3f)
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
        _controllerScript.uiUpdateLives(lives); // Update the UI with the new lives count
        if (lives <= 0)
        {
            // Debug.Log("Player has died.");
            StartCoroutine(Die());
        }
        else
        {
            // Debug.Log("Player took damage: " + damage + ", remaining lives: " + lives);
            // Optionally play a damage sound or animation here
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
            // PLazerScript bulletScript = newBullet.GetComponent<PLazerScript>();
            // if (bulletScript != null) bulletScript.damage = 1; 
        }
        _audioSource.Play();
    }
    
    private IEnumerator Die()
    {
        Debug.Log("Player has died. Starting death sequence...");
        speed = 0.5f;
        GameObject newAnim = Instantiate(explosionPrefab, transform.position + new Vector3(0f, 0f, -3f), Quaternion.identity);
        newAnim.transform.localScale = transform.localScale * 0.8f;

        for (int i = 0; i < 30; i++)
        {
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-0.7f, 0.7f), UnityEngine.Random.Range(-0.7f, 0.7f), -3);
            transform.position += randomOffset * 0.1f;
            bool eType = true; // true for mini explosion, false for big explosion
            if (i % 3 == 0) eType = false; // every third
            GameObject explosionPrefab = eType ? miniExplosionPrefab : this.explosionPrefab;
            GameObject explosion = Instantiate(explosionPrefab, transform.position + randomOffset, Quaternion.identity);
            explosion.transform.localScale = eType ? transform.localScale : transform.localScale * 0.3f;
            yield return new WaitForSeconds(0.1f);
        }
        GameObject finalExplosion = Instantiate(explosionPrefab, transform.position + new Vector3(0f, 0f, -3f), Quaternion.identity);
        GameObject finalExplosion2 = Instantiate(miniExplosionPrefab, transform.position + new Vector3(0f, 0f, -3f), Quaternion.identity);
        finalExplosion2.transform.localScale = transform.localScale * 20f;
        Destroy(gameObject);
    }
}
