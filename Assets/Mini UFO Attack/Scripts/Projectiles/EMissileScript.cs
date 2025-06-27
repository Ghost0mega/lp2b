using UnityEngine;

public class EMissileScript : Projecile
{
    [Header("Special Settings")]
    [SerializeField] private GameObject contactEffect;
    public Transform playerTransform;

    protected override void Update()
    {
        if (playerTransform != null)
        {

            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            direction = directionToPlayer;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer) * Quaternion.Euler(0, 0, -90f);
        }
        // if (timer <= 0f)
        // {
        //     ContactDestroy();
        //     return;
        // }
        base.Update();
    }

    public override void ContactDestroy()
    {
        if (contactEffect != null)
        {
            Instantiate(contactEffect, transform.position, Quaternion.identity);
        }
        base.ContactDestroy();
    }
}
