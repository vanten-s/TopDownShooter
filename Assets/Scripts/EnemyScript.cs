using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyScript : MonoBehaviour
{
    public float speed;

    private PlayerScript player;
    private Rigidbody2D rigidbodyComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        rigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 direction = playerPos - (Vector2)transform.position;
        float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }

    void FixedUpdate()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 direction = (playerPos - (Vector2)transform.position).normalized;

        rigidbodyComponent.linearVelocity = speed * Time.fixedDeltaTime * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision!");
        if (collision.gameObject.CompareTag("bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            // Setting health automatically triggers the event OnAttack
            player.GetComponent<PlayerScript>().health -= 1;
            Destroy(gameObject);
            return;
        }
    }
}
