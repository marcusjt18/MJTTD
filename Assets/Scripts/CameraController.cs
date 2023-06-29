using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panMinLimit;
    public Vector2 panMaxLimit;
    public float scrollSpeed = 20f;
    public float minZoom = 4f;
    public float maxZoom = 9f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        //if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        //{
        //    pos.y += panSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        //{
        //    pos.y -= panSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        //{
        //    pos.x += panSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        //{
        //    pos.x -= panSpeed * Time.deltaTime;
        //}

        if (Input.GetKey("w"))
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newSize = Camera.main.orthographicSize - scroll * scrollSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);

        pos.x = Mathf.Clamp(pos.x, panMinLimit.x, panMaxLimit.x);
        pos.y = Mathf.Clamp(pos.y, panMinLimit.y, panMaxLimit.y);

        transform.position = pos;
    }
}



