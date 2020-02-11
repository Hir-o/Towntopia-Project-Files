using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   private Camera camera;
   private Board board;
   private Transform parent;

   //The starting point where the player clicks
   private Vector3 mouseOriginPoint;

    //Used to offset the camera to the opposite direction from where the player drags
    private Vector3 offset;
    private Vector3 clampedPosition;
    private bool isDragging;

    [SerializeField] float minHeight = 3f;
    [SerializeField] float maxHeight = 30f;

    [SerializeField] float zoomFactor = 1f;
    [SerializeField] float zoomSpeedFactor = .1f;

    //Camera movement with keys
    [SerializeField] float panSpeed = 30f;

    [Header("Zoom Keys")]
    [SerializeField] private float zoomValue = 5f;

    private Vector3 forward, right;

   void Awake()
   {
        forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;    

        camera = Camera.main;
        parent = GetComponentInParent<Transform>();
   }

   private void LateUpdate() {
       //Changes the size of the camera to give the effect of zooming in or out, and clamps it
       camera.orthographicSize = Mathf.Clamp(camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") 
       * (zoomFactor * camera.orthographicSize * zoomSpeedFactor), minHeight, maxHeight);

       if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Z))
       {
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize -= zoomValue 
            * (zoomFactor * camera.orthographicSize * zoomSpeedFactor), minHeight, maxHeight);
       }

       if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.X))
       {
           camera.orthographicSize = Mathf.Clamp(camera.orthographicSize += zoomValue 
            * (zoomFactor * camera.orthographicSize * zoomSpeedFactor), minHeight, maxHeight);
       }

       if(Input.GetMouseButton(2))
       {
           //This line of code calculates the dragging value and adds it to the offset
           offset = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

           if(!isDragging)
           {
               isDragging = true;
               mouseOriginPoint = camera.ScreenToWorldPoint(Input.mousePosition);
           }
       }
       else
       {
           isDragging = false;
       }

       if(isDragging)
       {
           clampedPosition = mouseOriginPoint - offset;
           transform.position = new Vector3(
               Mathf.Clamp(clampedPosition.x, -40f, 40f),
               Mathf.Clamp(clampedPosition.y, 20f, 60f),
               Mathf.Clamp(clampedPosition.z, -40f, 40f)
               );
       }

       if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(forward * panSpeed * Time.deltaTime, Space.World);

            if (transform.position.x < -40f)
            {
                transform.position = new Vector3(-40f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 40f)
            {
                transform.position = new Vector3(40f, transform.position.y, transform.position.z);
            }
            if (transform.position.y < 20f)
            {
                transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
            }
            if (transform.position.y > 60f)
            {
                transform.position = new Vector3(transform.position.x, 60f, transform.position.z);
            }
            if (transform.position.z < -40f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -40f);
            }
            if (transform.position.z > 40f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 40f);
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(-forward * panSpeed * Time.deltaTime, Space.World);

            if (transform.position.x < -40f)
            {
                transform.position = new Vector3(-40f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 40f)
            {
                transform.position = new Vector3(40f, transform.position.y, transform.position.z);
            }
            if (transform.position.y < 20f)
            {
                transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
            }
            if (transform.position.y > 60f)
            {
                transform.position = new Vector3(transform.position.x, 60f, transform.position.z);
            }
            if (transform.position.z < -40f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -40f);
            }
            if (transform.position.z > 40f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 40f);
            }
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-right * panSpeed/2 * Time.deltaTime, Space.World);

            if (transform.position.x < -40f)
            {
                transform.position = new Vector3(-40f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 40f)
            {
                transform.position = new Vector3(40f, transform.position.y, transform.position.z);
            }
            if (transform.position.y < 20f)
            {
                transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
            }
            if (transform.position.y > 60f)
            {
                transform.position = new Vector3(transform.position.x, 60f, transform.position.z);
            }
            if (transform.position.z < -40f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -40f);
            }
            if (transform.position.z > 40f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 40f);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(right * panSpeed/2 * Time.deltaTime, Space.World);

            if (transform.position.x < -40f)
            {
                transform.position = new Vector3(-40f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 40f)
            {
                transform.position = new Vector3(40f, transform.position.y, transform.position.z);
            }
            if (transform.position.y < 20f)
            {
                transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
            }
            if (transform.position.y > 60f)
            {
                transform.position = new Vector3(transform.position.x, 60f, transform.position.z);
            }
            if (transform.position.z < -40f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -40f);
            }
            if (transform.position.z > 40f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 40f);
            }
        } 
   }
}
