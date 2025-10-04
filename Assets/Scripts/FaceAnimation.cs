using UnityEngine;

public class FaceAnimation : MonoBehaviour
{
    public GameObject idle;
    public GameObject scared;

    public float scaredDelay = 0.25f;
    public float speedThreshold = 2.5f;

    private bool isGrounded = true;
    private float scaredTimer = 0f;
    private bool scaredTimerRunning = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ShowIdle();
    }

    private void Update()
    {
        if (rb != null && rb.linearVelocity.magnitude > speedThreshold)
        {
            scaredTimerRunning = false;
            ShowScared();
            return;
        }

        if (isGrounded)
        {
            scaredTimerRunning = false;
            ShowIdle();
            return;
        }

        if (!scaredTimerRunning)
        {
            scaredTimerRunning = true;
            scaredTimer = 0f;
        }
        else
        {
            scaredTimer += Time.deltaTime;
            if (scaredTimer >= scaredDelay)
            {
                ShowScared();
                scaredTimerRunning = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void ShowIdle()
    {
        if (idle != null) idle.SetActive(true);
        if (scared != null) scared.SetActive(false);
    }

    private void ShowScared()
    {
        if (idle != null) idle.SetActive(false);
        if (scared != null) scared.SetActive(true);
    }
}