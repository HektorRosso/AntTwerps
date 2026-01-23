using UnityEngine;

public class TuskedAnt : MonoBehaviour
{
    public float fallThresholdY = -7.5f;

    private GameChecker gameChecker;

    void Start()
    {
        gameChecker = FindFirstObjectByType<GameChecker>();
    }

    void Update()
    {
        if (transform.position.y < fallThresholdY)
        {
            if (gameChecker != null)
            {
                gameChecker.HandleTargetDestroyed(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Friend")
        {
            Destroy(collision.gameObject);

            if (collision.gameObject.tag == "Player")
                gameChecker.TriggerLoss("THE BLUE ANT GOT EATEN BY A TUSKED ANT");
            else
                gameChecker.TriggerLoss("A GREEN ANT GOT EATEN BY A TUSKED ANT");
        }
    }
}