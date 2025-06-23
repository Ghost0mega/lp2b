using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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
}
