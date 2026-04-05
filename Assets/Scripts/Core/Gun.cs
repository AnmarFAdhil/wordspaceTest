using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float projectileSpeed = 50f;
    
    private float timeSinceLastShot = 0f;

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        
        if (timeSinceLastShot >= fireRate)
        {
            FireForward();
            timeSinceLastShot = 0f;
        }
    }

    private void FireForward()
    {
        if (projectilePrefab == null || muzzlePoint == null)
            return;

        GameObject projectileInstance = Instantiate(projectilePrefab, muzzlePoint.position, Quaternion.identity);
        Projectile proj = projectileInstance.GetComponent<Projectile>();
        
        if (proj != null)
        {
            proj.SetVelocity(muzzlePoint.forward * projectileSpeed);
        }
        else
        {
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = muzzlePoint.forward * projectileSpeed;
        }
    }

    public void SetProjectilePrefab(GameObject prefab) => projectilePrefab = prefab;
    public void SetMuzzlePoint(Transform point) => muzzlePoint = point;
}
