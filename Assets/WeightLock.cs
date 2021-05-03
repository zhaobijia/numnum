using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightLock : MonoBehaviour
{
    PlayerController player;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            player = collision.gameObject.GetComponentInParent<PlayerController>();

            Invoke("PlayerWeightLock", 1f);
        }
    }

    void PlayerWeightLock()
    {
        player.weight_lock = true;
    }
}
