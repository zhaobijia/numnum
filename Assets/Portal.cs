using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform spawnPoint; //spawn point for this portal
    // Start is called before the first frame update
    [SerializeField] private int transportWeightMin;
   // [SerializeField] private int transportWeightMax;
    public Vector2 bounceForce;

    public BoxCollider2D collider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!collider.isTrigger)
        {
            Invoke("TriggerOn", 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckTransportState(other);
    }
 

    private void CheckTransportState(Collider2D other)
    {
        //check if collision is player, ignore all other things;
        if (other.gameObject.GetComponentInParent<PlayerController>()!=null)
        {
            PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();

            if (player.gained_weight < transportWeightMin)
            {
                //bounce: require 2 colliders
                //current collider turnoff trigger
                collider.isTrigger = false;
                //not very bouncy, how bout add a force to left on player
                //player.m_Rigidbody2D.AddForce(bounceForce);
            
            }else if(player.gained_weight>=transportWeightMin)
            {
                //transport
                player.gained_weight = transportWeightMin;

                if (player.weight_lock)
                {
                    player.weight_lock = false;
                }

                FindObjectOfType<GameManager>().isInTransition = true;

                player.TransportAnime();

                player.currentPortal = this;

                FindObjectOfType<AudioManager>().Play("transport");
            }
            
        }
        else
        {
            //ignore
        }
    }
  


    private void TriggerOn()
    {
        collider.isTrigger = true;
    }
}
