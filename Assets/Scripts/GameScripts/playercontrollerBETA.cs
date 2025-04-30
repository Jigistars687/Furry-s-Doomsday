using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playercontrollerBETA : MonoBehaviour
{
    public float moveSpeed = 5f;  // Скорость движения
    public float jumpForce = 5f;  // Сила прыжка
    public float mouseSensitivity = 2f;  // Чувствительность мыши

    private Rigidbody rb;
    public Camera playerCamera;
    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("Main camera not found. Please ensure there is a camera with the 'MainCamera' tag in the scene.");
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerCamera == null) return;

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move * moveSpeed;

        velocity.y = rb.velocity.y;  // Сохраняем вертикальную скорость для прыжка

        rb.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
