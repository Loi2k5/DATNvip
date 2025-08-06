using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingUI;       // UI Panel chính
    public Image fillImage;            // Ô Image thanh fill (Value)
    public TextMeshProUGUI percentText; // Text hiển thị % (Text (TMP))
    public float loadDuration = 3f;    // Thời gian chạy loading

    private void Start()
    {
        if (loadingUI != null && fillImage != null && percentText != null)
        {
            loadingUI.SetActive(true);
            fillImage.fillAmount = 0f;
            StartCoroutine(FillLoadingBar());
        }
        else
        {
            Debug.LogError("Chưa gán đủ các thành phần UI.");
        }
    }

    IEnumerator FillLoadingBar()
    {
        float elapsedTime = 0f;
        while (elapsedTime < loadDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = Mathf.Clamp01(elapsedTime / loadDuration);
            fillImage.fillAmount = percent;
            percentText.text = Mathf.RoundToInt(percent * 100f) + "%";
            yield return null;
        }

        // Đảm bảo 100% chính xác khi kết thúc
        fillImage.fillAmount = 1f;
        percentText.text = "100%";

        yield return new WaitForSeconds(0.5f); // chờ 0.5s rồi ẩn UI
        loadingUI.SetActive(false);
    }
}
