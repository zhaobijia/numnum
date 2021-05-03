using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingCircle : MonoBehaviour
{
    public Camera camera;
    public Transform currentPlayer;//cameras' target
    public bool IsClosing;
    public Animator anim;
    public SpriteRenderer spRenderer;


    private void Update()
    {
        //if menu ask for closing
        if (IsClosing)
        {
            Close();

            IsClosing = false;
        }
    }

    public void Close()
    {
        spRenderer.enabled = true;
        currentPlayer = camera.GetComponent<CameraController>().target;

        //position changes along with player/ center player
        transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, 0);

        //scale changes along with camera.
        FindObjectOfType<CameraController>().target = null;

    }

    public void ChangeScale()
    {
        float factor = camera.orthographicSize / 5f;

        transform.localScale = new Vector3(transform.localScale.x * factor, transform.localScale.y * factor, transform.localScale.z);


    }


}
