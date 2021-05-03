using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int[] deathLayers;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            //Destroy(other.gameObject.transform.parent.gameObject);
            //死亡UI往这里看！！！
            other.gameObject.GetComponentInParent<PlayerController>().PlayerDeath();
           // other.gameObject.GetComponentInParent<PlayerController>().bodyAnimator.enabled = true;
           // other.gameObject.GetComponentInParent<PlayerController>().bodyAnimator.SetTrigger("SMALL");

            
        }


        if (other.gameObject.GetComponentInParent<EnemyInfo>() != null)
        {
            Destroy(other.gameObject.GetComponentInParent<EnemyInfo>().gameObject);
        }
        else
        {
            foreach (int i in deathLayers)
            {
                if (other.gameObject.layer == i)
                {
                    Destroy(other.gameObject);
                }
            }
        }
        

    }
}
