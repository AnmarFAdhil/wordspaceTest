using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float health = 30f;
    [SerializeField] private float damageInterval = 1f;
    
    private Rigidbody rb;
    private Transform playerTransform;
    private float timeSinceLastDamage = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = FindObjectOfType<PlayerController>()?.transform;
    }

    private void FixedUpdate()
    {
        if (playerTransform == null)
            return;

        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
        rb.velocity = dirToPlayer * moveSpeed;
    }

    private void Update()
    {
        timeSinceLastDamage += Time.deltaTime;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (timeSinceLastDamage >= damageInterval && collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                timeSinceLastDamage = 0f;
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
