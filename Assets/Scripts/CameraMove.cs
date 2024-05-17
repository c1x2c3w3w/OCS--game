using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    float Minzoom, Maxzoom, zoom;
    float MovingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Minzoom = -70;
        Maxzoom = -5;
        MovingSpeed = 0.05f;
        // the starting zoom
        zoom = -40;
        AdjustZoom(zoom);
    }

    // Update is called once per frame
    void Update()
    {
        float xDelta = Input.GetAxis("Horizontal");
        float yDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || yDelta != 0f)
        {
            AdjustPosition(xDelta, yDelta);
        }
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }
    }
    public void AdjustPosition(float xDelta, float yDelta)
    {
        if (transform.position.x > 38)
        {
            transform.localPosition = new Vector3((float)(transform.position.x - 0.1), transform.position.y, transform.position.z);
            return;
        }
        else if (transform.position.x < -25)
        {
            transform.localPosition = new Vector3((float)(transform.position.x + 0.1), transform.position.y, transform.position.z);
            return;
        }
        if (transform.position.y > 20)
        {
            transform.localPosition = new Vector3((float)(transform.position.x), (float)(transform.position.y - 0.1), transform.position.z);
            return;
        }
        else if (transform.position.y < -34)
        {
            transform.localPosition = new Vector3((float)(transform.position.x), (float)(transform.position.y + 0.1), transform.position.z);
            return;
        }

        transform.Translate(new Vector3(xDelta * MovingSpeed, yDelta * MovingSpeed));
    }
    void AdjustZoom(float Delta)
    {
        zoom = Mathf.Clamp01(zoom + Delta);
        float distance = Mathf.Lerp(Minzoom, Maxzoom, zoom);
        float x = transform.position.x;
        float y = transform.position.y;
        transform.localPosition = new Vector3(x, y, distance);
    }
   
}
