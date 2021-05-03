using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float m_JumpForce;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
 //   [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
 //   [SerializeField] private Transform m_CeilingCheck;

    [SerializeField] private CircleCollider2D groundCollider;



 //   const int maxSize = 3; //all 
    public int gained_weight = 0;

    public int starting_weight;

    public int starting_level;

    public bool weight_lock ;//set l1 to false, l2/l3/l4 to true

    private int current_level = 0;

    public GameObject nextLevelPlayer;

    public GameObject levelUpAnimation;

    [SerializeField]private float k_GroundedRadius = .5f;
    public bool m_Grounded;
//    const float k_CeilingRadius = .2f;
    public Rigidbody2D m_Rigidbody2D;
    private bool m_facingRight = true;
    private Vector3 m_Velocity = Vector3.zero;




    public Animator bodyAnimator;
    [SerializeField] private Transform m_EatingCheck;
    [SerializeField] private LayerMask m_WhatToEat;
    public float k_EatingRadius; //modify this 


    public CameraController cameraController;

  

    //scale
    Vector3 originalSize;
    Vector3 nextSize;

                                                                                                                                                                                                                                                                                                                                                                                                             

    


    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    public UnityEvent InMouthEvent;

    Vector3 currentScale;

    private void Awake()
    {
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if(OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        if(InMouthEvent == null)
        {
            InMouthEvent = new UnityEvent();
        }

        if(cameraController == null)
        {
    
            cameraController = Camera.main.GetComponent<CameraController>();
        }
        cameraController.target = transform;

        cameraController.orginalCamSize = cam1;

        cameraController.mainCamera.orthographicSize = cam1;

        cameraController.offset = cameraOffset1;


        gained_weight = starting_weight;

        //current_level = starting_level;

        m_JumpForce = jump1;

        m_Rigidbody2D.gravityScale = gravity1;

        

        deathParticle.Stop();
    }


    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;

        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
  
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i] != groundCollider )
            {//exempt from the collider on the player 
                m_Grounded = true;

                if (!wasGrounded)

                    OnLandEvent.Invoke();
            }
        }

        if (scale)
        {
           originalSize =  PlayerScaleUp(originalSize, nextSize);
        }

        if (cameraController.zoom)
        {
            cameraController.orginalCamSize = cameraController.ScaleCamera(cameraController.orginalCamSize, cameraController.nextCamSize);
        }
    }


    bool runningSoundOn;
    public void Move(float move, bool jump)
    {   
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (move >0 && !m_facingRight)
        {
            FaceRight();

        }
        else if(move<0 && m_facingRight)
        {
            FaceLeft();
        }
        else
        {
            //where move = 0 
            //face wherever it was facing 

        }
        

        if(m_Grounded && jump)
        {
            m_Grounded = false;

            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

        if(Mathf.Abs(move)>0.01f && m_Grounded)
        {
            if (!runningSoundOn)
            {
                FindObjectOfType<AudioManager>().Play("running");
                runningSoundOn = true;
            }
        }
        else
        {
            runningSoundOn = false;
            FindObjectOfType<AudioManager>().Stop("running");
        }
    }

    private void FaceRight()
    {
        m_facingRight = true;

        Vector3 theScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        transform.localScale = theScale;
    }

    private void FaceLeft()
    {
        m_facingRight = false;

        Vector3 theScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        theScale.x *= -1;
        transform.localScale = theScale;

    }
    private void Flip()
    {


        m_facingRight = !m_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Jump()
    {

    }



    [SerializeField] SpriteRenderer spRenderer;
    [SerializeField] Sprite openMouthSprite;
    [SerializeField] Sprite closeMouthSprite;

    private EnemyInfo encountered_enemy;

    public void OpenMouth()
    {
        spRenderer.sprite = openMouthSprite;


        FindObjectOfType<AudioManager>().Play("open");
    }
    public void Eat()
    {
        spRenderer.sprite = closeMouthSprite;
        //1. animation
        //eatingAnimator.SetTrigger("EAT");
        //2. check circle overlap with colliders
        Collider2D[] in_colliders = Physics2D.OverlapCircleAll(m_EatingCheck.position, k_EatingRadius, m_WhatToEat);
        for (int i = 0; i < in_colliders.Length; i++)
        {
            
            if (in_colliders[i].gameObject != gameObject )
            {
               
                //if (in_colliders[i].gameObject.GetComponent<EnemyInfo>() != null)
                //{
                   
                //    encountered_enemy = in_colliders[i].GetComponent<EnemyInfo>();

                //    if (encountered_enemy.eaten_weight < gained_weight)
                //    {
                //        InMouthEvent.Invoke(); //event (Grow())

                //        FinishEat();
                //    }


                //}
                //else 
                if (in_colliders[i].gameObject.GetComponentInParent<EnemyInfo>())
                {
                    encountered_enemy = in_colliders[i].GetComponentInParent<EnemyInfo>();

                    if (encountered_enemy.eaten_weight <= gained_weight)
                    {
                        InMouthEvent.Invoke(); //event 

                        FinishEat();
                        print("eat function after finish eat");
                        
                    }
                }

                print("weight : "+gained_weight);
                break;
            }
        }

        //FinishEat();

        
         
    }

    //Functions that gets a callback at InMouthEvent Invoking.
    [Space(10)]
    public float size1; // the oooooriginal set
    public float size2;
    public float size3;
    public float size4;
    [Space(10)]
    public float cam1;
    public float cam2;
    public float cam3;
    public float cam4;
    [Space(10)]
    public float jump1;
    public float jump2;
    public float jump3;
    public float jump4;
    [Space(10)]
    public float gravity1;
    public float gravity2;
    public float gravity3;
    public float gravity4;
    [Space(10)]
    public Vector3 cameraOffset1;
    public Vector3 cameraOffset2;
    public Vector3 cameraOffset3;
    public Vector3 cameraOffset4;
    [Space(10)]
    public int growWeight1;
    public int growWeight2;
    public int growWeight3;
    public int growWeight4;
    public int levelupWeight;

    bool isSize2, isSize3, isSize4;

    public void Grow()
    {
        if (encountered_enemy != null)
        {
            if (!weight_lock)
            {
                gained_weight += encountered_enemy.weight;

                if (gained_weight >= growWeight2 && (gained_weight<growWeight3) &&(!isSize2))
                {
                    FindObjectOfType<AudioManager>().Play("grow");
                    isSize2 = true;
                    print("1 :"+gained_weight);

                    originalSize = new Vector3(size1, size1, 0);
                    nextSize = new Vector3(size2, size2, 0);

                    scale = true;


                    //camera scale up
                    //cameraController.mainCamera.orthographicSize = cameraController.mainCamera.orthographicSize * 1.44f;
                    cameraController.orginalCamSize = cameraSizeAtPortal;
                    cameraController.nextCamSize = cam2;
                    cameraController.zoom = true;

                    cameraController.InitiateOnNewTarget();

                    //ZoomWaitCoroutine();
                    //radius scale up
                    k_EatingRadius = k_EatingRadius * 1.6f;
                    k_GroundedRadius = k_GroundedRadius * 1.6f;

                    //Jump Force
                    m_JumpForce = jump2;
                    //Gravity
                    m_Rigidbody2D.gravityScale = gravity2;

                    //CameraOffset
                    cameraController.offset = cameraOffset2;

                }
                else if (gained_weight >= growWeight3 && (gained_weight<growWeight4) && (!isSize3))
                {
                    FindObjectOfType<AudioManager>().Play("grow");
                    isSize3 = true;
                    print("2 :" + gained_weight);
                    originalSize = new Vector3(size2, size2, 0);
                    nextSize = new Vector3(size3, size3, 0);

                    scale = true;


                    //camera scale up

                    cameraController.orginalCamSize = cam2;
                    cameraController.nextCamSize = cam3;

                    cameraController.zoom = true;

                    cameraController.InitiateOnNewTarget();
                    // ZoomWaitCoroutine();
                    //radius scale up
                    k_EatingRadius = k_EatingRadius * 1.6f;
                    k_GroundedRadius = k_GroundedRadius * 1.6f;

                    //Jump Force
                    m_JumpForce = jump3;
                    //Gravity
                    m_Rigidbody2D.gravityScale = gravity3;

                    //CameraOffset
                    cameraController.offset = cameraOffset3;
                }
                else if (gained_weight >= growWeight4 && (gained_weight<levelupWeight) && (!isSize4))
                {
                    FindObjectOfType<AudioManager>().Play("grow");
                    isSize4 = true;
                    print("3 :" + gained_weight);

                    originalSize = new Vector3(size3, size3, 0);
                    nextSize = new Vector3(size4, size4, 0);

                    scale = true;


                    //camera scale up

                    cameraController.orginalCamSize = cam3;
                    cameraController.nextCamSize = cam4;

                    cameraController.zoom = true;

                    cameraController.InitiateOnNewTarget();
                    //ZoomWaitCoroutine();
                    //radius scale up
                    k_EatingRadius = k_EatingRadius * 1.6f;
                    k_GroundedRadius = k_GroundedRadius * 1.6f;

                    //Jump Force
                    m_JumpForce = jump4;
                    //Gravity
                    m_Rigidbody2D.gravityScale = gravity4;

                    //CameraOffset
                    cameraController.offset = cameraOffset4;
                }
                else if (gained_weight >= levelupWeight)
                {
                    FindObjectOfType<AudioManager>().Play("levelup");
                    print("4 :" + gained_weight);
                    LevelUp();
                    Destroy(gameObject, 0.1f);
                }
            }
            
        }
    }
    
    private void LevelUp()
    {
        if(encountered_enemy != null)
        {
            
            GameObject levelupO = Instantiate(levelUpAnimation, this.transform.position, this.transform.rotation);
           // levelupO.transform.parent = transform;
            //change camera
            
            
            
            GameObject nextPlayer = Instantiate(nextLevelPlayer, this.transform.position, this.transform.rotation);

            PlayerController p = nextLevelPlayer.GetComponent<PlayerController>();

            PlayerMovement pM = nextLevelPlayer.GetComponent<PlayerMovement>();
            print(pM);
            pM.playerControl = true;
            //change camera
            cameraController.target = nextPlayer.transform;
            
            cameraController.InitiateOnNewTarget();

            //cameraController.offset = new Vector3(5f, cameraOffsetY, -10f);




            Destroy(levelupO, 0.25f);
            
            

            
        }
    }




    public void FinishEat()
    {
        //set animation back to default

    
        if (encountered_enemy != null)
        {
            //check weight
            
                encountered_enemy.GotEaten();

                FindObjectOfType<AudioManager>().Play("playereat");
            
        }
        encountered_enemy = null; 


    }


    bool scale = false;
    public Vector3 PlayerScaleUp(Vector3 originalSize, Vector3 nextSize)
    {
        transform.localScale = Vector3.Lerp(originalSize, nextSize, 0.2f);
        
        originalSize = transform.localScale;//!!!! this is a problem

        if(nextSize.x - transform.localScale.x < 0.1f)
        {
            scale = false;
        }
        m_facingRight = true;
        return originalSize;
    }

    public void GotEaten()
    {
        //Destroy(gameObject);

        PlayerDeath();
    }

    public void TransportAnime()
    {
        currentScale = transform.localScale;
        print("currentScale "+currentScale);
        bodyAnimator.enabled = true;

        bodyAnimator.SetTrigger("SMALL");
    }
    [Space(10)]
    public Portal currentPortal;

    float transportSpeed = 30f;


    public float cameraSizeAtPortal;
    public Vector3 cameraOffsetAtPortal;
    
    public void TransportOnPortal()
    {
        if (currentPortal != null)
        {
            print("transport on " + currentPortal);

            transform.position = currentPortal.spawnPoint.position;

            cameraController.transform.position = Vector3.Lerp(cameraController.transform.position, currentPortal.spawnPoint.position, transportSpeed * Time.deltaTime);

            //camera scale up

            cameraController.orginalCamSize = cam1;
            cameraController.nextCamSize = cameraSizeAtPortal;

            cameraController.zoom = true;

            //CameraOffset
            cameraController.offset = cameraOffsetAtPortal;
        }

        bodyAnimator.SetTrigger("IDLE");

        transform.localScale = currentScale;

        //bodyAnimator.enabled = false;
    }



    //player death


    public ParticleSystem deathParticle;


    public void PlayerDeath()
    {
        //play death circle
        //deathCircle.gameObject.SetActive(true);

        //deathCircle.IsClosing = true;
        //bodyAnimator.enabled = true;

        FindObjectOfType<GameManager>().EndGame();



        //bodyAnimator.SetTrigger("SMALL");

        deathParticle.Play();

        deathParticle.transform.parent = null;

        Destroy(deathParticle.gameObject, 0.5f);

        Destroy(gameObject, 0.04f);
    }



}
