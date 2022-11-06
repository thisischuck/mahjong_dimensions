using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera camera;
    Vector3 mousePosition;

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

        }
    }
}
