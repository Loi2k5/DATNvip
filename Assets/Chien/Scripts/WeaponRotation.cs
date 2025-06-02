using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public Transform weapon; // Kéo object khẩu súng vào đây
    public Vector2 moveDirection;

    void Update()
    {
        if (moveDirection.x > 0.01f)
        {
            // Đi phải
            weapon.localEulerAngles = new Vector3(0f, 0f, 0f);
            weapon.localPosition = new Vector3(0.5f, 0f, 0f);
        }
        else if (moveDirection.x < -0.01f)
        {
            // Đi trái
            weapon.localEulerAngles = new Vector3(0f, 180f, 0f);
            weapon.localPosition = new Vector3(-0.5f, 0f, 0f);
        }
        else if (moveDirection.y > 0.01f)
        {
            // Đi lên
            weapon.localEulerAngles = new Vector3(0f, 180f, 90f);
            weapon.localPosition = new Vector3(0.5f, 0f, 0f);
        }
        else if (moveDirection.y < -0.01f)
        {
            // Đi xuống
            weapon.localEulerAngles = new Vector3(180f, 180f, 90f);
            weapon.localPosition = new Vector3(0.5f, 0f, 0f);
        }
    }
}
