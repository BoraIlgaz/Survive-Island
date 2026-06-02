using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Takip edilecek karakter (Oyuncuyu buraya sürükle)
    public float distance = 5.0f; // Karakterden uzaklýk
    public float xSpeed = 120.0f; // Fare hassasiyeti X
    public float ySpeed = 120.0f; // Fare hassasiyeti Y

    public float yMinLimit = -20f; // Aþaðý bakma sýnýrý
    public float yMaxLimit = 80f;  // Yukarý bakma sýnýrý

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Fare imlecini oyun ekranýnýn ortasýna kilitle ve gizle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target)
        {
            // Fare hareketlerini al
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            // Kameranýn yukarý/aþaðý takla atmasýný engellemek için sýnýrla
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);

            // Karakterin biraz yukarýsýna odaklanmasý için (baþ hizasý)
            Vector3 targetPosition = target.position + Vector3.up * 1.5f;

            Vector3 position = rotation * negDistance + targetPosition;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
}