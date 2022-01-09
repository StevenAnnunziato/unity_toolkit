using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsEyeCamera : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 0.17f;
    public float screenEdgeX = 0.83f;
    public float screenEdgeY = 0.75f;
    public float screenMovementAccelleration = 1.5f;
    public Vector2 boundsX = new Vector2(-20, 20);
    public Vector2 boundsY = new Vector2(-20, 20);
    public Vector2 scrollBounds = new Vector2(3, 14);
    public float zoomSpeed = 3f;

    // private member variables
    private float mouseX;
    private float mouseY;
    private Vector3 currentDirection;
    private Vector2 speedModifier;
    private float defMoveSpeed;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        defMoveSpeed = moveSpeed;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        // movement on x axis
        if (mouseX > Screen.width * screenEdgeX)
        {
            currentDirection.x = 1f;
            speedModifier.x = mouseX - Screen.width * screenEdgeX;
        }
        else if (mouseX < Screen.width * (1f - screenEdgeX))
        {
            currentDirection.x = -1f;
            speedModifier.x = Screen.width * (1f - screenEdgeX) - mouseX;
        }
        else
        {
            currentDirection.x = 0f;
            speedModifier.x = 1f;
        }

        // movement on y axis
        if (mouseY > Screen.height * screenEdgeY)
        {
            currentDirection.y = 1f;
            speedModifier.y = mouseY - Screen.height * screenEdgeY;
        }
        else if (mouseY < Screen.height * (1f - screenEdgeY))
        {
            currentDirection.y = -1f;
            speedModifier.y = Screen.height * (1f - screenEdgeY) - mouseY;
        }
        else
        {
            currentDirection.y = 0f;
            speedModifier.y = 1f;
        }

        speedModifier.x = Remap(speedModifier.x, 0f, Screen.width - (Screen.width * screenEdgeX), 0f, 100f * screenMovementAccelleration);
        speedModifier.y = Remap(speedModifier.y, 0f, Screen.height - (Screen.height * screenEdgeY), 0f, 100f * screenMovementAccelleration);

        Vector3 finalDirection = new Vector3(currentDirection.normalized.x * speedModifier.x, currentDirection.normalized.y * speedModifier.y, 0f);
        transform.Translate(finalDirection * moveSpeed * Time.deltaTime);

        // clamp position
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, boundsX.x, boundsX.y);
        pos.y = Mathf.Clamp(transform.position.y, boundsY.x, boundsY.y);
        transform.position = pos;

        // re-center the camera when the player presses space
        if (Input.GetKeyDown(KeyCode.Space)) {
            transform.position = new Vector3(0f, 0f, transform.position.z);
        }

        // scroll in and out
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel != 0f)
        {
            cam.orthographicSize -= wheel * zoomSpeed;
        }
        // clamp zooming
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, scrollBounds.x, scrollBounds.y);

    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void HaltMovement()
    {
        moveSpeed = 0f;
    }
    public void ResumeMovement()
    {
        moveSpeed = defMoveSpeed;
    }
}
