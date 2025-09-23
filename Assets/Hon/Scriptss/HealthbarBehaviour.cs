using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehaviour : MonoBehaviour
{
    [Header("UI Elements")]
    public Image BackgroundImage; // ?nh n?n thanh máu
    public Image FillImage;       // ?nh máu (type = Filled, fill method = Horizontal)

    [Header("Settings")]
    public Color Low = Color.red;
    public Color High = Color.green;
    public Vector3 Offset = new Vector3(0, 2, 0);

    private float maxHealth = 100f;

    public void SetHealth(float health, float maxHealth)
    {
        this.maxHealth = maxHealth;

        if (FillImage == null)
        {
            Debug.LogError("FillImage is not assigned on " + gameObject.name);
            return;
        }

        float fillAmount = Mathf.Clamp01(health / maxHealth);

        FillImage.fillAmount = fillAmount;
        FillImage.color = Color.Lerp(Low, High, fillAmount);

        gameObject.SetActive(health < maxHealth);
    }

    void Update()
    {
        if (Camera.main != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
        }
    }
}
