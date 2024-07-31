using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform target; // Target yang akan diikuti kamera (biasanya player)
    public Transform cameraTransform; // Transform dari kamera itu sendiri

    [Header("Camera Settings")]
    public float distanceFromTarget = 5f;
    public float heightOffset = 2f;
    public float positionDamping = 5f; 
    public float rotationDamping = 10f;

    private void LateUpdate() 
    {
        if (target == null) return; // Keluar jika target belum ditetapkan

        // Hitung posisi target kamera di belakang target
        Vector3 targetPosition = target.position - target.forward * distanceFromTarget;
        targetPosition.y += heightOffset; // Sesuaikan ketinggian kamera

        // Smoothing pergerakan kamera
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, positionDamping * Time.deltaTime);

        // Smoothing rotasi kamera
        Quaternion targetRotation = Quaternion.LookRotation(target.position - cameraTransform.position);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, rotationDamping * Time.deltaTime);
    }
}
