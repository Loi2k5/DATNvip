using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemShop : MonoBehaviour
{
    public GameObject petPrefab;
    public string petID;
    public int cost = 1;

    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI nameText;

    void Start()
    {
        if (petPrefab != null) petID = petPrefab.name;
        nameText.text = petID;

        buyButton.onClick.AddListener(OnBuyClicked);
        buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mua (" + cost + ")";
    }

    void OnBuyClicked()
    {
        if (PointManager.Instance.CurrentPoints >= cost)
        {
            PointManager.Instance.AddPoint(-cost);
            PetPurchaseManager.Instance.AddPet(petID); // thêm vào list tạm thời
            Debug.Log("Mua thành công: " + petID);

            // đổi text nút thành "Đã mua"
            buyButton.interactable = false; // khóa nút lại
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Đã mua";
        }
        else
        {
            Debug.Log("Không đủ điểm để mua: " + petID);
        }
    }

}
