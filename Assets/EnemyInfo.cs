using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInfo : MonoBehaviour
{
    public int weight;
    public int eaten_weight;

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    public float m_JumpForce;
    public LayerMask m_WhatIsGround;
    public Transform m_GroundCheck;
    [SerializeField] protected CircleCollider2D groundCollider;
    
    public float k_GroundedRadius = .2f;
    public bool m_Grounded;
    public Rigidbody2D m_Rigidbody2D;
    private bool m_facingRight = true;
    public Vector3 m_Velocity = Vector3.zero;
    public float runSpeed;

    

    //Enemy Eat other ai or player
    public Animator eatingAnimator;
    [SerializeField] private Transform m_EatingCheck;
    [SerializeField] private LayerMask m_WhatToEat;
    public float k_EatingRadius; //modify this

    //Open Mouth Check
    //area that the ai can perform an open Mouth
    [SerializeField] private Transform m_OpenMouthCheck;
    [SerializeField] private LayerMask m_WhatToOpenMouth;
    public float k_OpenMouthRadius;

    //searching for other edible and player

    public Transform m_EnemyCheck;
    [SerializeField] private LayerMask m_WhatIsEnemy;
    public float k_searchRadius;

    bool isVisible = false;


    //chasing
    public bool isJump;
    public bool isFoward;

    
    //public UnityEvent InMouthEvent;


    public void GotEaten()
    {
        
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

 

    public void Move(float move, bool jump)
    {
        if (m_Grounded)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_facingRight)
            {
                Flip();

            }
            else if (move < 0 && m_facingRight)
            {
                Flip();
            }
        }

        if (m_Grounded && jump)
        {
            m_Grounded = false;

            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    public void Flip()
    {
        m_facingRight = !m_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public bool IsOpenMouth = false;
    public void OpenMouthCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_OpenMouthCheck.position, k_OpenMouthRadius, m_WhatToOpenMouth);

        for(int i = 0; i<colliders.Length; i++)
        {
            //the 
            if (colliders[i].gameObject != gameObject)
            {//not colliding with self but something on the layer mask
                float dice = Random.Range(0f, 1f);
                if (dice > 0.4)
                {
                    IsOpenMouth = true;

         
                }
                
            }
        }


    
    
    
    }

    private EnemyInfo encountered_enemy;
    [SerializeField]private PlayerController encoutered_player;


    public SpriteRenderer spRenderer;
    public Sprite openMouthSprite;
    public Sprite closeMouthSprite;
    public void Eat()
    {



        //1. animation
       eatingAnimator.SetTrigger("EAT");
        //replace this with open mouth sprite
       
   
        //2. check circle overlap with colliders
        Collider2D[] in_colliders = Physics2D.OverlapCircleAll(m_EatingCheck.position, k_EatingRadius, m_WhatToEat);

        for (int i = 0; i < in_colliders.Length; i++)
        {

            if (in_colliders[i].gameObject != gameObject)
            {//if 

                if (in_colliders[i].gameObject.GetComponentInParent<EnemyInfo>() != null)
                {

                    encountered_enemy = in_colliders[i].GetComponentInParent<EnemyInfo>();
                    //encoutered_player = in_colliders[i].GetComponentInParent<PlayerController>();

                   // InMouthEvent.Invoke(); //event 
                }

                if(in_colliders[i].gameObject.GetComponentInParent<PlayerController>() != null)
                {
                    encoutered_player = in_colliders[i].GetComponentInParent<PlayerController>();



                   // InMouthEvent.Invoke(); //event 
                }
            }
        }


        //back to idle no matter what?
        //FinishEat();



    }

    //this is close mouth
    public void FinishEat()
    {
        //set animation back to default

         eatingAnimator.SetTrigger("IDLE");
        //replace this with close mouth sprite



        if (encountered_enemy != null)
        {

            encountered_enemy.GotEaten();

            if (isVisible)
            {
                FindObjectOfType<AudioManager>().Play("eating");
            }
        }
        if(encoutered_player != null)
        {
            if (encoutered_player.gained_weight < weight)
            {
                encoutered_player.GotEaten();

                FindObjectOfType<AudioManager>().Play("eating");
            }
        }

        encountered_enemy = null;

        

    }


    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    //As enemy check for player and other objects
    //enemymovement才控制他动。。所以这个function要在movement里面被call
    //那么这个function可不可以return一个vector3 或者transform之类的 好一帧一帧搞啊
    //这个是一个要在update里call的东西啊！！！好气哦
    //详情见blackthornprod的视频。




    public bool FoundPlayer;
    public bool PlayerIsEdible;
    public Transform playerTransform;
    public void Search()
    {
        FoundPlayer = false;
        PlayerIsEdible = false;
        // priority 1. player 2.other things
        //returns collider that in selected layers that collides with enemy search
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_EnemyCheck.position, k_searchRadius, m_WhatIsEnemy);

        for(int i =0; i < colliders.Length; i++)
        {
            //If hit player：
            if (colliders[i].gameObject.GetComponentInParent<PlayerController>() != null)
            {
                FoundPlayer = true;

                if(colliders[i].gameObject.GetComponentInParent<PlayerController>().gained_weight < weight)
                {
                    PlayerIsEdible = true;
                }
                playerTransform = colliders[i].gameObject.transform;
               // Chase(colliders[i].gameObject.transform, transform);
            }
        }
        

    }

    public void Chase(Transform player, Transform monster)
    {
        isJump = false;
        isFoward = false;
        
        if (player.position.y >= monster.position.y)
        {
            //top

            //If player's at the top left of the circle then just jump
            if (player.position.x <= monster.position.x)
            {
                if (m_Grounded)
                {
                    isJump = true;
                }
            }
            else
            {
                //If player's at the top right of the circle then jump and right walk;
                if (m_Grounded)
                {
                    isJump = true;
                }
                isFoward = true;
            }
        }
        else if (player.position.y < monster.position.y)
        {
            //bottom

            //If player's at the bottom half of the circle then walk towards player
            //go towards the player
            isFoward = true;

        }
    }

}
