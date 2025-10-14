using TMPro;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxSpawns = 3;
    [SerializeField] private TextMeshProUGUI spawnCounterText;

    private GameObject currentPlayer;
    private int spawnCount = 0;
    private bool hasLost = false;
    private bool mouseOverButton = false;

    [HideInInspector] public GameChecker gameChecker;

    void Start()
    {
        UpdateSpawnUI();
    }

    void Update()
    {
        if (hasLost) return;

        Vector3? inputWorldPos = null;

        // Mouse input (desktop)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0f;
            inputWorldPos = worldPos;
        }
        // Touch input (mobile)
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

            if (pos.x >= -9f && pos.x <= 9f && pos.y >= -5f && pos.y <= 5f)
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log("Input is over an existing game object, no spawn.");
                    return;
                }

                if (spawnCount >= maxSpawns)
                {
                    hasLost = true;
                    if (gameChecker != null)
                        gameChecker.TriggerLoss("THE NUMBER OF CLICKS EXCEEDED THE MAX LIMIT");
                    else
                        Debug.LogWarning("GameChecker not assigned to SpawnPlayer!");
                    return;
                }

                if (currentPlayer != null)
                    Destroy(currentPlayer);

                currentPlayer = Instantiate(playerPrefab, pos, Quaternion.identity);
                spawnCount++;
                UpdateSpawnUI();
            }
            else
            {
                Debug.Log("Input was outside defined boundaries, no spawn.");
            }
        }
    }

    private void UpdateSpawnUI()
    {
        if (spawnCounterText != null)
        {
            int spawnsUsed = spawnCount;
            spawnCounterText.text = $"PAR: {spawnsUsed} / {maxSpawns}";
        }
    }
}