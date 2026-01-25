using UnityEngine;
using System.Collections;

public class FaceAnimation : MonoBehaviour
{
    [Header("Face GameObjects")]
    public GameObject idle;
    public GameObject scared;
    public GameObject hit;
    public GameObject happy;

    [Header("Scared Settings")]
    public float scaredDelay = 0.25f;
    public float speedThreshold = 2.5f;

    [Header("Hit Detection")]
    public float hitImpactThreshold = 7f;
    public float hitDisplayTime = 0.3f;

    [Header("Hit Sound Settings")]
    public AudioSource audioSource;
    public AudioClip hitSound;
    public float hitSoundPitch = 1f;

    private bool isGrounded = true;
    private float scaredTimer = 0f;
    private bool scaredTimerRunning = false;
    private Rigidbody2D rb;
    private Coroutine hitCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ShowIdle();
    }

    private void Update()
    {
        if (hit != null && hit.activeSelf)
            return;

        if (happy != null && happy.activeSelf)
            return;

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
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Friend") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Snail Shell"))
        {
            isGrounded = true;

            float impactForce = collision.relativeVelocity.magnitude;
            if (impactForce >= hitImpactThreshold)
            {
                TriggerHitReaction();
            }
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
        if (hit != null) hit.SetActive(false);
        if (happy != null) happy.SetActive(false);
    }

    private void ShowScared()
    {
        if (idle != null) idle.SetActive(false);
        if (scared != null) scared.SetActive(true);
        if (hit != null) hit.SetActive(false);
        if (happy != null) happy.SetActive(false);
    }

    private void ShowHit()
    {
        if (idle != null) idle.SetActive(false);
        if (scared != null) scared.SetActive(false);
        if (hit != null) hit.SetActive(true);
        if (happy != null) happy.SetActive(false);
    }
    public void ShowHappy()
    {
        if (idle != null) idle.SetActive(false);
        if (scared != null) scared.SetActive(false);
        if (hit != null) hit.SetActive(false);
        if (happy != null) happy.SetActive(true);
    }

    private void TriggerHitReaction()
    {
        if (hitCoroutine != null)
            StopCoroutine(hitCoroutine);

        PlayHitSound();
        hitCoroutine = StartCoroutine(ShowHitBriefly());
    }

    private IEnumerator ShowHitBriefly()
    {
        ShowHit();
        yield return new WaitForSeconds(hitDisplayTime);
        ShowIdle();
    }

    private void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.pitch = hitSoundPitch;
            audioSource.PlayOneShot(hitSound);
        }
    }
}