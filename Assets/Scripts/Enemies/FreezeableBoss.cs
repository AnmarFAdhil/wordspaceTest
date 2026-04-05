using UnityEngine;

public class FreezeableBoss : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float maxHealth = 200f;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private float freezeDuration = 3f;
    
    private Rigidbody rb;
    private Transform playerTransform;
    private float currentHealth;
    private float timeSinceLastDamage = 0f;
    private bool isFrozen = false;
    private float freezeTimeRemaining = 0f;
    
    private Color originalColor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = FindObjectOfType<PlayerController>()?.transform;
        currentHealth = maxHealth;
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
            originalColor = renderer.material.color;
    }

    private void FixedUpdate()
    {
        if (isFrozen || playerTransform == null)
            return;

        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
        rb.velocity = dirToPlayer * moveSpeed;
    }

    private void Update()
    {
        timeSinceLastDamage += Time.deltaTime;
        
        if (isFrozen)
        {
            freezeTimeRemaining -= Time.deltaTime;
            if (freezeTimeRemaining <= 0)
            {
                Unfreeze();
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Freeze()
    {
        isFrozen = true;
        freezeTimeRemaining = freezeDuration;
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = Color.cyan;
    }

    private void Unfreeze()
    {
        isFrozen = false;
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = originalColor;
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

    public float GetHealthPercent() => currentHealth / maxHealth;
}
