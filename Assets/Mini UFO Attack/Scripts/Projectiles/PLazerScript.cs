using UnityEngine;

public class PLazerScript : Projecile
{

    [Header("Special Settings")]
    
    [SerializeField] private GameObject contactEffect;

    override public void ContactDestroy()
    {
        if (contactEffect != null)
        {
            Instantiate(contactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
