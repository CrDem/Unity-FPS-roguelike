using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float delay = 3f; 
    public float explosionRadius = 5f; 
    public float maxDamage = 100f;
    public AnimationCurve damageFalloff; 

    [Header("Effects")]
    public GameObject explosionEffect; 

    private bool hasExploded = false; 

    void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                float damage = CalculateDamage(distance);

                var enemy = collider.GetComponent<Entity>();
                
                if (enemy != null)
                {
                    Debug.Log("damage");
                    enemy.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }

    float CalculateDamage(float distance)
    {
        if (distance <= 0f)
            return maxDamage;

        if (damageFalloff != null)
        {
            float normalizedDistance = Mathf.Clamp01(distance / explosionRadius);
            return maxDamage * damageFalloff.Evaluate(normalizedDistance);
        }

        return Mathf.Lerp(maxDamage, 0f, distance / explosionRadius);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
