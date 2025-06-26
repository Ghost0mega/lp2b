using UnityEngine;

public class Bringback : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetAsFirstSibling();
    }
}