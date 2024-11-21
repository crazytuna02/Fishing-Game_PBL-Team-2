using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCam : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan gerak
    public float jumpForce = 5f; // Kekuatan lompatan
    public float sensitivity = 100f; // Sensitivitas rotasi kamera

    public Transform cameraTransform; // Transform kamera untuk mengontrol rotasi

    private float xRotation = 0f; // Variabel untuk menyimpan rotasi sumbu X (vertikal)
    private float yRotation = 0f; // Variabel untuk menyimpan rotasi sumbu Y (horizontal)
    private Rigidbody rb; // Referensi ke Rigidbody untuk lompatan

    private bool cursorLocked = true; // Status kursor terkunci

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Mengambil komponen Rigidbody
        LockCursor(); // Mengunci kursor di awal permainan
    }

    // Update is called once per frame
    void Update()
    {
        RotateCameraWithMouse();

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        // Toggle cursor lock/unlock when left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            cursorLocked = !cursorLocked;
            if (cursorLocked)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void RotateCameraWithMouse()
    {
        if (!cursorLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
