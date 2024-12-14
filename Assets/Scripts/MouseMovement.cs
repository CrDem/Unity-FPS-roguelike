using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

using Input=UnityEngine.Input;
public class MouseMovement : MonoBehaviour
{

    public float mouseSens = 100f;

    private float xRotation = 0;
    private float yRotation = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= MouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += MouseX;
        
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
