using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] private AudioSource chatterAudioSource;
    [SerializeField] private AudioClip chatterClip;

    [Header("Volume Levels")]
    [SerializeField] private float level1Volume = 0.3f;  // Low
    [SerializeField] private float level2Volume = 0.6f;  // Medium
    [SerializeField] private float level3Volume = 1.0f;  // High

    private int currentLevel = 2; // Default to level 2

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (chatterAudioSource == null)
        {
            chatterAudioSource = GetComponent<AudioSource>();
            if (chatterAudioSource == null)
            {
                chatterAudioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Load audio clip if not assigned
        if (chatterClip == null)
        {
            chatterClip = Resources.Load<AudioClip>("ChatterSound"); // You can adjust the resource path
        }

        if (chatterAudioSource != null && chatterClip != null)
        {
            chatterAudioSource.clip = chatterClip;
            chatterAudioSource.loop = true;
            chatterAudioSource.playOnAwake = false;
        }
    }

    public void SetLevel(int level)
    {
        if (level < 1 || level > 3)
        {
            Debug.LogWarning("Level must be between 1 and 3");
            return;
        }

        currentLevel = level;

        float newVolume = level switch
        {
            1 => level1Volume,
            2 => level2Volume,
            3 => level3Volume,
            _ => level2Volume
        };

        if (chatterAudioSource != null)
        {
            chatterAudioSource.volume = newVolume;
            
            // Start playing if not already playing
            if (!chatterAudioSource.isPlaying && chatterClip != null)
            {
                chatterAudioSource.Play();
            }
        }

        Debug.Log($"Audio level set to {level} with volume {newVolume}");
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void PlayChatter()
    {
        if (chatterAudioSource != null && !chatterAudioSource.isPlaying)
        {
            chatterAudioSource.Play();
        }
    }

    public void StopChatter()
    {
        if (chatterAudioSource != null && chatterAudioSource.isPlaying)
        {
            chatterAudioSource.Stop();
        }
    }
}
