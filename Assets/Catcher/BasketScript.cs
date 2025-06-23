using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private float SPEED = 7.0f;
    private AudioSource _audio;
    [SerializeField] private AudioClip _clip;

    // Reference to ControllerScript instance
    [SerializeField] private ControllerScript controller;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.loop = false;
        _audio.volume = 1f;
        _audio.clip = _clip;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 7.26f)
        {
            transform.Translate(SPEED * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -7.26f)
        {
            transform.Translate(-SPEED * Time.deltaTime, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collectible")
        {
            Fruit fruit = collision.gameObject.GetComponent<Fruit>();
            if (fruit != null && controller != null)
            {
                controller.AddScore(fruit.value);
                _audio.Play();
            }
        }
    }
}
