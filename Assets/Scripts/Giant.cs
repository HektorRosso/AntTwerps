using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Giant : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private int contactHeavyCount = 0;
    private int boulderContactCount = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 1000000f;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    private void FixedUpdate()
    {
        if ((!IsGrounded() || contactHeavyCount == 0 || boulderContactCount > 0) && rb.constraints != RigidbodyConstraints2D.FreezeRotation)
        {
            UnfreezeGiant();
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Boulder"))
        {
            boulderContactCount++;
            UnfreezeGiant();
            return;
        }

        if (IsGrounded() && (collision.collider.CompareTag("Enemy") ||
                             collision.collider.CompareTag("Ally") ||
                             collision.collider.CompareTag("Player")))
        {
            contactHeavyCount++;
            if (boulderContactCount == 0)
            {
                FreezeGiant();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Boulder"))
        {
            boulderContactCount = Mathf.Max(0, boulderContactCount - 1);
            if (boulderContactCount == 0 && contactHeavyCount > 0)
            {
                FreezeGiant();
            }
            return;
        }

        if (IsGrounded() && (collision.collider.CompareTag("Enemy") ||
                             collision.collider.CompareTag("Ally") ||
                             collision.collider.CompareTag("Player")))
        {
            contactHeavyCount = Mathf.Max(0, contactHeavyCount - 1);

            if (contactHeavyCount == 0 || boulderContactCount > 0)
                UnfreezeGiant();
        }
    }

    private void FreezeGiant()
    {
        rb.mass = 1000000f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                         RigidbodyConstraints2D.FreezePositionY |
                         RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void UnfreezeGiant()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.mass = 0.05f;
    }
}