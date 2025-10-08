using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusic;
    public AudioClip background;
    public static BackgroundMusic instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        backgroundMusic.clip  = background;
    }
}
