using System;
using UnityEngine;

public class PickupScript : Projecile
{
    [SerializeField] private String pickupType;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.AddPickup(pickupType);
                // Debug.Log("Pickup collected by player.");
                ContactDestroy();
            }
            else
            {
                Debug.LogWarning("PlayerScript component not found on player GameObject.");
            }
        }
        else
        {
            base.OnCollisionEnter2D(collision);
        }
    }
}
