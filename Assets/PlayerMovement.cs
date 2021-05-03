using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController playerController;

    public bool playerControl;

    float horizontalMove = 0f;

    public float runSpeed = 40f;

    bool jump = false;
    bool openMouth = false;
    bool eat = false;

    //Walk/Idle/Jump Animation
    public Animator legAnimator;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (playerControl)
        {
            //where we get input from the player an
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            if (Mathf.Abs(horizontalMove) < 0.001f)
            {
                legAnimator.SetTrigger("IDLE");
            }
            else
            {
                legAnimator.SetTrigger("RUN");
            }

            if (Input.GetButtonDown("Jump") && playerController.m_Grounded)
            {
                legAnimator.SetTrigger("JUMP");
                jump = true;
            }

            if (Input.GetButtonDown("EAT"))
            {

                openMouth = true;
            }
            if (Input.GetButtonUp("EAT"))
            {
                eat = true;
            }
        }
        else
        {
            legAnimator.SetTrigger("IDLE");
        }
 
    }

    private void FixedUpdate()
    {
        if (playerControl)
        {
            playerController.Move(horizontalMove * Time.fixedDeltaTime, jump);
            //move the same amount no matter how often this function is called 
            jump = false;
            if (openMouth)
            {
                playerController.OpenMouth();

            }
            //the eating move
            if (eat)
            {
                playerController.Eat();
                openMouth = false;
                eat = false;

            }
        }
        


    }
}
