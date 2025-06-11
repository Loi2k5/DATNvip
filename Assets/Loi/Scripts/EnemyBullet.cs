using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 movementDirection;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (movementDirection == Vector3.zero) return;
        transform.position += movementDirection * Time.deltaTime;
    }
    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }
}
