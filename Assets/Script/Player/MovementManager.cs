using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform; // Referensi ke transform kamera
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Perbarui pergerakan karakter di FixedUpdate untuk fisika yang lebih stabil
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(movementInput);
    }

    public void Move(Vector2 movementInput)
    {
        // Dapatkan arah kamera di bidang horizontal
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // Hitung arah pergerakan berdasarkan input dan arah kamera
        Vector3 movementDirection = cameraForward * movementInput.y + cameraTransform.right * movementInput.x;

        // Gerakan
        rb.MovePosition(transform.position + movementDirection * moveSpeed * Time.fixedDeltaTime);

        // Rotasi karakter (hanya jika bergerak)
        if (movementInput != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 5f * Time.fixedDeltaTime); 
        }
    }
}
