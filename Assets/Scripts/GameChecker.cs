using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

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
    public GameObject loseCanvas;
    public TextMeshProUGUI reasonText;
    public TextMeshProUGUI countdownText;

    private List<GameObject> allies = new List<GameObject>();
    private int fallenAllies = 0;

    public AudioSource audioSource;
    public AudioClip tickSound;
    public AudioClip victoryMusic;
    public AudioClip defeatMusic;

    private int lastSecondPlayed = -1;

    void Start()
    {
        GameObject[] allyArray = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] fireAnts = GameObject.FindGameObjectsWithTag("FireAnt");

        allies.AddRange(allyArray);

        trackedObjects.AddRange(enemies);
        trackedObjects.AddRange(fireAnts);

        totalTargets = trackedObjects.Count;
        handledTargets = 0;

        Debug.Log("Tracking " + totalTargets + " enemies and fire ants.");
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

                if (reasonText != null)
                {
                    reasonText.text = "THE BLUE ANT FELL OFF THE SCREEN";
                }

                if (loseCanvas != null)
                    loseCanvas.SetActive(true);

                if (audioSource != null && defeatMusic != null)
                {
                    audioSource.Stop();
                    audioSource.clip = defeatMusic;
                    audioSource.loop = false;
                    audioSource.Play();
                }

                return;
            }
        }

        if (!playerHasFallen && allies.Count > 0)
        {
            List<GameObject> fallenAlliesList = new List<GameObject>();

            foreach (GameObject ally in allies)
            {
                if (ally != null && ally.transform.position.y < fallThresholdY)
                {
                    fallenAlliesList.Add(ally);
                }
            }

            foreach (GameObject fallenAlly in fallenAlliesList)
            {
                allies.Remove(fallenAlly);
                fallenAllies++;

                if (fallenAllies >= 1)
                {
                    playerHasFallen = true;
                    Debug.Log("An ally fell. Game Over!");
                    Time.timeScale = 0f;

                    if (reasonText != null)
                        reasonText.text = "A GREEN ANT FELL OFF THE SCREEN";

                    if (loseCanvas != null)
                        loseCanvas.SetActive(true);

                    if (audioSource != null && defeatMusic != null)
                    {
                        audioSource.Stop();
                        audioSource.clip = defeatMusic;
                        audioSource.loop = false;
                        audioSource.Play();
                    }

                    return;
                }
            }
        }

        if (countdownActive && !hasWon && !playerHasFallen)
        {
            countdownTimer -= Time.deltaTime;

            if (countdownText != null)
            {
                int secondsLeft = Mathf.CeilToInt(countdownTimer);
                countdownText.text = secondsLeft.ToString();

                if (secondsLeft != lastSecondPlayed && secondsLeft > 0)
                {
                    if (audioSource != null && tickSound != null)
                    {
                        audioSource.PlayOneShot(tickSound);
                    }
                    lastSecondPlayed = secondsLeft;
                }
            }

            if (countdownTimer <= 0f)
            {
                hasWon = true;
                Debug.Log("Player survived after handling all targets. You win!");
                Time.timeScale = 0f;
                UnlockNewLevel();

                if (winCanvas != null)
                    winCanvas.SetActive(true);

                if (countdownText != null)
                    countdownText.text = "";

                if (audioSource != null && victoryMusic != null)
                {
                    audioSource.Stop();
                    audioSource.clip = victoryMusic;
                    audioSource.loop = false;
                    audioSource.Play();
                }

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