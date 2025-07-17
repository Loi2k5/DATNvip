using UnityEngine;

/// <summary>
/// Lưu trữ các cờ (flags) trạng thái toàn cục cho trò chơi.
/// Dùng để xác định các sự kiện như boss đã chết.
/// </summary>
public static class GameFlags
{
    // Biến cờ để xác định boss đã bị hạ chưa
    public static bool bossIsDead = false;
    public static bool rescuedParents = false;
    public static bool hasUltimateWeapon = false;
    public static bool secretUnlocked = false;
    public static bool metShinParents = false;
}
