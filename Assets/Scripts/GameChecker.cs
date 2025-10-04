using UnityEngine;
using System.Collections.Generic;

public class GameChecker : MonoBehaviour
{
    public float fallThresholdY = -5f;

    private List<GameObject> trackedObjects = new List<GameObject>();
    private int totalTargets;
    private int handledTargets;

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
                Debug.Log("All enemies and fire ants handled! You win!");
            }
        }
    }
}