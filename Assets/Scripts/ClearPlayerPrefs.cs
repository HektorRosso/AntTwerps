using UnityEngine;

public class ClearPlayerPrefs : MonoBehaviour
{
    public void ClearAllPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs cleared!");
    }
    
    /*
    void Start()
    {
        ClearAllPrefs();
    }
    */
}