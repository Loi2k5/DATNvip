using UnityEngine;
using UnityEngine.Tilemaps;

public class HouseHide : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tilemapRenderer.enabled = false; // Ẩn nhà
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tilemapRenderer.enabled = true; // Hiện lại nhà
        }
    }
}
