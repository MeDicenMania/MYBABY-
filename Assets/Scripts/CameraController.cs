using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target;
    public float heightOffset = 1.5f;

    [Header("config de orbita")]
    public float distance = 6f; 
    public float mouseSensitivityX = 3f;
    public float mouseSensitivityY = 3f;

    [Header("Limites verticales")]
    public float minYAngle;
    public float maxYAngle;

    private float currentX;
    private float currentY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        currentX += Input.GetAxis("Mouse X") * mouseSensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivityY;

        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        Vector3 targetCenter = target.position + Vector3.up * heightOffset;
        Vector3 position = targetCenter - (rotation * Vector3.forward * distance);

        transform.position = position;
        transform.rotation = rotation;
    }
}