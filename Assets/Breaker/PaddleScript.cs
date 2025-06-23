using UnityEngine;

public class Padle : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -4)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 4)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
