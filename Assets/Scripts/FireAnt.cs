using UnityEngine;

public class FireAnt : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public float explosionForce = 10f;
    public float explosionRadius = 2.5f;

    void OnMouseDown()
    {
        Explode();
    }

    void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in colliders)
        {
            Rigidbody2D rb = col.attachedRigidbody;
            if (rb != null && rb != GetComponent<Rigidbody2D>())
            {
                Vector2 direction = col.transform.position - transform.position;
                rb.AddForce(direction.normalized * explosionForce);
            }
        }
        Destroy(gameObject);
    }
}