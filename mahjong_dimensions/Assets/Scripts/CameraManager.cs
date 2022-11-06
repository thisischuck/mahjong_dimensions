using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float ratio = 1f;

    Camera camera;
    Vector3 mousePosition;
    public GameObject baseLevel;

    Vector3 initialMouse;

    // Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            initialMouse = mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            float difference = (mousePosition - initialMouse).magnitude;

            if (mousePosition.x > Screen.currentResolution.width / 2)
                baseLevel.transform.Rotate(Vector3.up, difference * ratio * Time.deltaTime, Space.World);
            else
                baseLevel.transform.Rotate(Vector3.up, -difference * ratio * Time.deltaTime, Space.World);
            //this.gameObject.transform.LookAt(Vector3.zero, Vector3.up);
        }
    }
}
