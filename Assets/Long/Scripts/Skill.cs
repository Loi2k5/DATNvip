using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public string description;
    public Sprite icon;

    // Nếu cần hiệu ứng khi chọn kỹ năng
    public void Apply()
    {
        Debug.Log($"Kỹ năng {skillName} đã được kích hoạt!");
        // TODO: logic nâng cấp thực tế ở đây
    }
}
