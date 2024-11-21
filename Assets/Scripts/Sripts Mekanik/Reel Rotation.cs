using UnityEngine;

public class RotateReel : MonoBehaviour
{
    public Transform pivotPoint; // Titik pusat rotasi
    public float rotationSpeed = 360f; // Kecepatan rotasi dalam derajat per detik
    private bool isRotating = false;

    void Update()
    {
        // Input dari joystick (Horizontal dan Vertical adalah default mapping untuk Joystick 2)
        float horizontalInput = Input.GetAxis("RightJoystickHorizontal");
        float verticalInput = Input.GetAxis("RightJoystickVertical");

        // Input sementara dari keyboard (spasi)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRotating = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isRotating = false;
        }

        // Periksa apakah joystick atau keyboard aktifkan rotasi
        if (isRotating || Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            // Hitung arah rotasi dari joystick
            Vector3 rotationDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;

            // Jika menggunakan joystick, gunakan input sebagai kecepatan rotasi
            if (rotationDirection != Vector3.zero)
            {
                transform.RotateAround(pivotPoint.position, rotationDirection, rotationSpeed * Time.deltaTime);
            }
            // Jika menggunakan keyboard, tetap gunakan transform.forward
            else if (isRotating)
            {
                transform.RotateAround(pivotPoint.position, transform.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
