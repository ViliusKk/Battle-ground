using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, 10);
    public float rotationSpeed = 5f;
    public float scrollSensitivity = 2f;

    private float currentX;
    private float currentY;
    private float currentZoom;
    
    void Start()
    {
        currentZoom = offset.z;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        currentX = Input.GetAxis("Mouse X") * rotationSpeed;
        currentY = Input.GetAxis("Mouse Y") * rotationSpeed;
        
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom += scroll * scrollSensitivity;
    }

    void LateUpdate()
    {
        if (!target) return;
        
        var rotation = Quaternion.Euler(currentY, currentX, 0);
        var direction = new Vector3(0, 0, currentZoom);
        var desiredPosition = target.position + rotation * direction;
        transform.position = desiredPosition + offset;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
