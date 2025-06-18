using UnityEngine;

public class PetFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(1f, 1f, 0f);
    public float followSpeed = 5f;
    private SpriteRenderer spriteRenderer;

    void Update()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        // Flip theo hướng player
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
}
