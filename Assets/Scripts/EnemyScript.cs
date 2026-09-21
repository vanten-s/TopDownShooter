using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;

    private GameObject player;
    private Rigidbody2D rigidbodyComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 direction = playerPos - (Vector2)transform.position;
        float lookAngle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }

    void FixedUpdate()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 direction = (playerPos - (Vector2)transform.position).normalized;

        rigidbodyComponent.linearVelocity = Time.fixedDeltaTime * direction * speed;
    }
}
