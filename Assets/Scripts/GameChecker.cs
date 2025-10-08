using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameChecker : MonoBehaviour
{
    public float fallThresholdY = -5f;

    private List<GameObject> trackedObjects = new List<GameObject>();
    private int totalTargets;
    private int handledTargets;

    private GameObject player;
    private bool playerHasFallen = false;

    private bool countdownActive = false;
    private float countdownTimer = 0f;
    private float winCountdownDuration = 5f;
    private bool hasWon = false;

    public GameObject winCanvas;

    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] fireAnts = GameObject.FindGameObjectsWithTag("FireAnt");

        trackedObjects.AddRange(enemies);
        trackedObjects.AddRange(fireAnts);

        totalTargets = trackedObjects.Count;
        handledTargets = 0;

        Debug.Log("Tracking " + totalTargets + " total objects.");
    }

    void Update()
    {
        if (player == null && !playerHasFallen)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null && !playerHasFallen)
        {
            if (player.transform.position.y < fallThresholdY)
            {
                playerHasFallen = true;
                Debug.Log("Player has fallen. Game Over!");
                Time.timeScale = 0f;
                return;
            }
        }

        if (countdownActive && !hasWon && !playerHasFallen)
        {
            countdownTimer -= Time.deltaTime;

            if (countdownTimer <= 0f)
            {
                hasWon = true;
                Debug.Log("Player survived after handling all targets. You win!");
                Time.timeScale = 0f;
                UnlockNewLevel();

                if (winCanvas != null)
                    winCanvas.SetActive(true);

                return;
            }
        }

        List<GameObject> fallenObjects = new List<GameObject>();

        foreach (GameObject obj in trackedObjects)
        {
            if (obj == null) continue;

            if (obj.transform.position.y < fallThresholdY)
            {
                fallenObjects.Add(obj);
            }
        }

        foreach (GameObject fallen in fallenObjects)
        {
            HandleTargetDestroyed(fallen);
        }
    }

    public void HandleTargetDestroyed(GameObject obj)
    {
        if (trackedObjects.Contains(obj))
        {
            trackedObjects.Remove(obj);
            handledTargets++;

            Debug.Log(obj.name + " handled. Remaining: " + (totalTargets - handledTargets));

            if (handledTargets >= totalTargets)
            {
                Debug.Log("All enemies and fire ants handled! Starting 5 second countdown...");
                countdownActive = true;
                countdownTimer = winCountdownDuration;
            }
        }
    }

    private void UnlockNewLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel + 1 > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
            Debug.Log("New level unlocked: " + (currentLevel + 1));
        }
    }
}