using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Camera Limits")]
    public float topClamp = 90f;
    public float bottomClamp = -90f;

    private float xRotation = 0f;
    [SerializeField] float touchSensitivity = 0.12f;

    void Start()
    {
        GameManager.SetGameplayCursor(true);
    }

    void Update()
    {
        if (Input.touchCount > 0)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        ApplyLook(mouseX, mouseY);
    }

    public void ApplyTouchLook(Vector2 screenDelta)
    {
        ApplyLook(screenDelta.x * touchSensitivity, screenDelta.y * touchSensitivity);
    }

    void ApplyLook(float horizontal, float vertical)
    {
        xRotation -= vertical;
        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * horizontal);
    }
}
