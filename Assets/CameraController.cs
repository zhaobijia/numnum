
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f; //how fast camera will follow

    public Vector3 offset;

    public Camera mainCamera;

    public bool zoom =false;
    [HideInInspector]
    public float orginalCamSize;
    [HideInInspector]
    public float nextCamSize;

    Rigidbody2D rb;
    // Vertical Direction Camera Boundary and Offset, following [Press Start Tutorial]

    private void Start()
    {
        threshold = calculateThreshold();

        rb = target.GetComponent<Rigidbody2D>(); //this should be called each time you change the target
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            //by the time of this function is called our target is already finished its movement
           // Vector3 desiredPosition = target.position + offset; //vector adding

           // Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
           

            float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * target.position.x);
            float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * target.position.y);

            Vector3 newPosition = transform.position;//new position stays where it is now

            if (Mathf.Abs(xDifference) >= threshold.x)
            {
                // transform.position = smoothedPosition;
                newPosition.x = target.position.x + offset.x; //onlt when xDiff is greater than threshold, it moves to target position
            }

            if (Mathf.Abs(yDifference) >= threshold.y)
            {
                //transform.position = smoothedPosition;
                newPosition.y = target.position.y +offset.y;
            }
            newPosition.z = -10f;


            if (FindObjectOfType<GameManager>().isInTransition)
            {
                //for other target
                Vector3 desiredPosition = target.position + offset; //vector adding

                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

                transform.position = smoothedPosition;
            }
            else
            {


                if (rb != null)
                {//for normal player
                    float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;


                    Vector3 desiredPosition = newPosition; //+ offset; //vector adding

                    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

                    transform.position = Vector3.MoveTowards(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
                }
                else
                {
                    //for other target
                    Vector3 desiredPosition = target.position + offset; //vector adding

                    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

                    transform.position = smoothedPosition;

                }
            }
        }
       


    }
 
    
    
    public float ScaleCamera(float originalCam, float nextCam)
    {
        //edge check
        mainCamera.orthographicSize = Mathf.Lerp(originalCam, nextCam, 0.2f);

        originalCam = mainCamera.orthographicSize;

        if(nextCam - mainCamera.orthographicSize < 0.1f)
        {
            zoom = false;
        }

        return originalCam;

        
        //mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, mainCamera.orthographicSize * factor, smoothSpeed);
    }


    // Vertical Direction Camera Boundary and Offset, following [Press Start Tutorial]
    public Vector2 followOffset;
    private Vector2 threshold;

    public float speed = 30f;
    private Vector3 calculateThreshold() {

        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }

    public void InitiateOnNewTarget()
    {
        threshold = calculateThreshold();

        if (target.GetComponent<Rigidbody2D>() != null)
        {
            rb = target.GetComponent<Rigidbody2D>();
        }
    }
}
