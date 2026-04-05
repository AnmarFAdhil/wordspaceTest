using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 5f;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = velocity;
        }
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
        if (rb != null)
            rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Health health = collision.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }

        EnemyAI enemy = collision.GetComponent<EnemyAI>();
        if (enemy != null && health == null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        FreezeableBoss boss = collision.GetComponent<FreezeableBoss>();
        if (boss != null && health == null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
