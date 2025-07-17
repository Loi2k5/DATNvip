using UnityEngine;

public class ShinParentsAppear : MonoBehaviour
{
    [SerializeField] private GameObject shinParents; // GameObject chứa cả bố mẹ

    private bool appeared = false;

    void Update()
    {
        if (!appeared && GameFlags.bossIsDead)
        {
            shinParents.SetActive(true); // Hiện NPC
            appeared = true;
        }
    }
}
