using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemShop : MonoBehaviour
{
    public string petID; // ID để spawn (tên prefab hoặc key)
    public int cost = 1;

    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI nameText;

    void Start()
    {
        buyButton.onClick.AddListener(OnBuyClicked);
        nameText.text = petID;
    }

    void OnBuyClicked()
    {
        if (PointManager.Instance.CurrentPoints >= cost)
        {
            PointManager.Instance.AddPoint(-cost);
            PlayerPrefs.SetString("SelectedPet", petID); // Lưu pet đã mua
            PlayerPrefs.Save();
            Debug.Log("Mua thành công: " + petID);
        }
        else
        {
            Debug.Log("Không đủ điểm");
        }
    }
}
