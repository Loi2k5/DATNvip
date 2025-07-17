using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    public int CurrentPoints { get; private set; }

    [SerializeField] private TextMeshProUGUI pointText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ lại giữa các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddPoint(int amount)
    {
        CurrentPoints += amount;
        UpdateUI();
    }

    public void SetPoints(int amount)
    {
        CurrentPoints = amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (pointText != null)
        {
            pointText.text = $" {CurrentPoints}";
        }
    }

    public void RefindPointText()
    {
        GameObject pointObj = GameObject.Find("PointText");
        if (pointObj != null)
        {
            pointText = pointObj.GetComponent<TextMeshProUGUI>();
            UpdateUI(); // cập nhật lại sau khi tìm thấy
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefindPointText(); // Tự tìm lại Text trong scene mới
    }

}
