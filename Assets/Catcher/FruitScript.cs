using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    const float BAS_ECRAN = -7.0f;

    public int value = 1;
    private float angularSpeed = 0f;
    [SerializeField] private float MaxAngularSpeed = 180f;
    [SerializeField] private Sprite[] fruitsSprites;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (fruitsSprites != null && fruitsSprites.Length > 0)
        {
            int index = Random.Range(0, fruitsSprites.Length);
            spriteRenderer.sprite = fruitsSprites[index];
            switch (index)
            {
                case 0:
                    value = 1; // Cherry
                    break;
                case 1:
                    value = 2; // Strawberry
                    break;
                case 2:
                    value = 3; // Purple
                    break;
                case 3:
                    value = 5; // Lemon
                    break;
                case 4:
                    value = 10; // Apple
                    break;
                case 5:
                    value = 15; // Orange
                    break;
                case 6:
                    value = 20; // Banana
                    break;
                case 7:
                    value = 30; // Pineapple
                    break;
                case 8:
                    value = 40; // Watermelon
                    break;
            }
        }
        angularSpeed = Random.Range(-MaxAngularSpeed, MaxAngularSpeed); // Random rotation speed
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, angularSpeed * Time.deltaTime);
        if (transform.position.y < BAS_ECRAN)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Basket")
        {
            Destroy(gameObject);
        }

    }
}
