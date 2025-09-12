using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    private float moveUpSpeed = 2f;
    private float disappearTimer = 1f;
    private Color textColor;

    public void Setup(float damageAmount)
    {
        textMesh.text = damageAmount.ToString();
        textColor = textMesh.color;
    }

    private void Update()
    {
        // Cho text bay lên
        transform.position += new Vector3(0, moveUpSpeed * Time.deltaTime, 0);

        // Đếm thời gian biến mất
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= 3f * Time.deltaTime; // mờ dần
            textMesh.color = textColor;

            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
