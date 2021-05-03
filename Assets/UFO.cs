using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{

    public BoxCollider2D lightCollider;

    public Animator ufoAnimator;

    PlayerController player;

    bool playerIsIn;

    bool playerIsUp;

    public float smoothSpeed;

    public GameObject line, light;

    public CameraController cameraController;
    private void LateUpdate()
    {
        if (playerIsIn)
        {
            Vector3 desiredP = new Vector3(transform.position.x, player.transform.position.y, player.transform.position.z);

            Vector3 smoothedP = Vector3.Lerp(player.transform.position, desiredP, smoothSpeed);

            player.transform.position = smoothedP;
        }

        if (playerIsUp)
        {
            Vector3 desiredP = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);

            Vector3 smoothedP = Vector3.Lerp(player.transform.position, desiredP, smoothSpeed);

            player.transform.position = smoothedP;
        }

        if (cameraController.zoom)
        {
            cameraController.orginalCamSize = cameraController.ScaleCamera(cameraController.orginalCamSize, cameraController.nextCamSize);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            PlayerMovement playerMove = collision.gameObject.GetComponentInParent<PlayerMovement>();
            print("playercontroller detected");
            //1. player controller lose control
            playerMove.playerControl = false;
            //2. light down anime
            ufoAnimator.SetTrigger("LIGHTDOWN");
            //3. player position centre, lerp it to middle[lerp in late update]
            playerIsIn = true;

            player = collision.gameObject.GetComponentInParent<PlayerController>();

            FindObjectOfType<GameManager>().escAvailable = false;
           // Vector3 pos = new Vector3(transform.position.x,collision.gameObject.GetComponentInParent<PlayerController>().transform.position.y, collision.gameObject.GetComponentInParent<PlayerController>().transform.position.z);
           // collision.gameObject.GetComponentInParent<PlayerController>().transform.position = pos;
        }
    }

    public void PlayerUp()
    {
        playerIsUp = true;

        Invoke("LinesOff", 0.2f);

        //camera change target
        FindObjectOfType<CameraController>().target = this.transform;

        Invoke("PlayerOff", 0.2f);

        //destroy player
        Destroy(player.gameObject, 0.25f);

        Invoke("UFOFollow", 0.3f);

        Invoke("UFOIdle", 2f);

        Invoke("EndScene", 2.5f);

        FindObjectOfType<AudioManager>().Play("ufo");


    }

    void LinesOff()
    {
        light.SetActive(false);

        line.SetActive(false);

        playerIsIn = false;
    }

    void PlayerOff()
    {
        playerIsUp = false;
    }
    
    
    void UFOFollow()
    {
        GetComponent<UFOFollow>().StartRoute();
    }

    void UFOIdle()
    {
        ufoAnimator.SetTrigger("IDLE");
    }

    public Transform endingWords;

    public Vector3 endingCameraOffset;

    public float endingCameraSize;

    
    void EndScene()
    {
        
        cameraController.target = endingWords;

        cameraController.orginalCamSize = cameraController.mainCamera.orthographicSize;
        cameraController.nextCamSize =endingCameraSize;
        cameraController.zoom = true;

        cameraController.offset = endingCameraOffset;

        FindObjectOfType<GameManager>().ShowRestartMenu();


    }
}
