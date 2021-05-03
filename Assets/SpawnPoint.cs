
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    PlayerController player;

    PlayerMovement playerMove;

    public float cameraSize;

    public Vector2 spawnPointCameraFollowOffset;



    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            player = collision.gameObject.GetComponentInParent<PlayerController>();

            playerMove = collision.gameObject.GetComponentInParent<PlayerMovement>();


            //if there is a camera zoom

            FindObjectOfType<GameManager>().isInTransition = false;

            player.cameraController.orginalCamSize = player.cameraController.mainCamera.orthographicSize;
            
            player.cameraController.nextCamSize = cameraSize;

            player.cameraController.zoom = true;

            player.cameraController.followOffset = spawnPointCameraFollowOffset;
            player.cameraController.InitiateOnNewTarget();
            playerMove.playerControl = true;

            Invoke("TrunOffPlayerAnimator", 0.5f);

            FindObjectOfType<GameManager>().Level++;
            //turn this thing off

            gameObject.SetActive(false);
        }
    }

    private void TrunOffPlayerAnimator()
    {
        player.bodyAnimator.enabled = false;
    }
}
