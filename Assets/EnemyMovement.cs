using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    //[Range(-1f,1f)]float horizontalMove = 0f;// -1 to 1

    public float runSpeed;
    //..........Idle and Patrol..........
    private bool patrolRight = true;

    public Transform groundDetect;
    public Transform obstacleDetect;

    public bool isIdling;
    public bool isPatroling;
    //.............Eat................
    public bool isOpenMouth;
    //............Chase.............                    



    //Walk/Idle/Jump Animation
    public Animator legAnimator;


    [SerializeField] float lookingAroundRate;
    // Start is called before the first frame update
    void Start()
    {
        runSpeed = enemyInfo.runSpeed;

        //only when Idling
        //InvokeRepeating("LookAround", 2.0f, lookingAroundRate);
    }

    // Update is called once per frame
    void Update()
    {
        //check the coditions to do transition
        //idle /patrol on a regular bases
        //if find a player inside the searching circle
        
        enemyInfo.Search();
        //...Search到有两种情况 1. 追 然后  吃 2.跑
        if (enemyInfo.FoundPlayer)
        {
            //chase or flee
            if (enemyInfo.PlayerIsEdible)
            {
                //chase
                Chasing();
            }
            else
            {
                //flee
                Fleeing();
            }

        }
        else
        {



            //....这是没得search到的情况
            if (isIdling)
            {
                Idling();

            }
            else if (isPatroling)
            {
                CancelInvoke(); 
                //stop invokerepeating
                Patroling();
            }
        }

        if (enemyInfo.IsOpenMouth)
        {
            Eating();
        }

    }

    //try to put all the action here first

    public void Idling()
    {
        //1. stay at the same position
        //transform is read only
        legAnimator.SetTrigger("IDLE");
        //3. idling animation
        InvokeRepeating("LookAround", 2.0f, lookingAroundRate);
        
        isIdling = false;
    }

    void LookAround()
    {
        if (Random.Range(0f, 1f) > 0.5)
        {
            enemyInfo.Flip();
        }
    }

    [SerializeField] private float groundDetectDistance;
    
    [SerializeField] private float obstacleDetectDistance;
    
    public void Patroling()
    {
        //1. need a route
        legAnimator.SetTrigger("RUN");

        transform.Translate(Vector2.right *runSpeed*Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, groundDetectDistance,enemyInfo.m_WhatIsGround);

        RaycastHit2D obstacleInfo = Physics2D.Raycast(obstacleDetect.position, Vector2.right, obstacleDetectDistance, enemyInfo.m_WhatIsGround);

        if(!groundInfo.collider && enemyInfo.m_Grounded)
        {
            Flip();
            
        }

        else if(obstacleInfo.collider != false && enemyInfo.m_Grounded)
        {
            //jump at the same time run??

            Flip();
            
        }

    }

    void Flip()
    {
        if (patrolRight)
        {

            transform.eulerAngles = new Vector3(0, -180, 0);

            patrolRight = false;
        }
        else
        {

            transform.eulerAngles = new Vector3(0, 0, 0);

            patrolRight = true;
        }
    }

    void Jump()
    {
        legAnimator.SetTrigger("JUMP");

        enemyInfo.m_Rigidbody2D.AddForce(new Vector2(0f,enemyInfo.m_JumpForce));
    }
    
    public void Eating()
    {
        //call when open mouth is checked true
        enemyInfo.Eat();

    }

    public void Fleeing()
    {
        legAnimator.SetTrigger("RUN");

        if ((enemyInfo.playerTransform.position.x - transform.position.x) > 0)
        {
            //look left
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);

        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-enemyInfo.playerTransform.position.x, transform.position.y, transform.position.z), runSpeed * Time.deltaTime);
    }

    public void Chasing()
    {//dont for
        legAnimator.SetTrigger("RUN");

        enemyInfo.Chase(enemyInfo.playerTransform, enemyInfo.m_EnemyCheck);
        
        if (enemyInfo.isJump)
            {

                Jump();
            }

            if (enemyInfo.isFoward)
            {
                if ((enemyInfo.playerTransform.position.x - transform.position.x) < 0)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);

                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);

                }
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(enemyInfo.playerTransform.position.x, transform.position.y, transform.position.z), runSpeed * Time.deltaTime);
            }

        }
}
