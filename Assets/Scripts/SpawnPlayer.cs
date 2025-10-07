using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private GameObject currentPlayer;

    void Update()
    {
        Vector3? inputWorldPos = null;

        // Check for mouse input (desktop)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0f;
            inputWorldPos = worldPos;
        }
        // Check for touch input (mobile)
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPos = touch.position;
                touchPos.z = 0f;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);
                worldPos.z = 0f;
                inputWorldPos = worldPos;
            }
        }

        if (inputWorldPos.HasValue)
        {
            Vector3 pos = inputWorldPos.Value;

            // Check boundaries
            if (pos.x >= -9f && pos.x <= 9f && pos.y >= -5f && pos.y <= 5f)
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log("Input is over an existing game object, no spawn.");
                    return;
                }

                if (currentPlayer != null)
                    Destroy(currentPlayer);

                currentPlayer = Instantiate(playerPrefab, pos, Quaternion.identity);
            }
            else
            {
                Debug.Log("Input was outside defined boundaries, no spawn.");
            }
        }
    }
}