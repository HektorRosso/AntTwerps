using UnityEngine;

public class GameChecker : MonoBehaviour
{
    private int enemiesRemaining;

    void Start()
    {
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            enemiesRemaining--;

            if (enemiesRemaining <= 0)
            {
                Debug.Log("All enemies have fallen! You win!");
            }
        }
    }
}