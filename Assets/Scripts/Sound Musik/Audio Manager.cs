using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance
    public AudioSource backgroundMusic;  // AudioSource untuk musik latar

    private void Awake()
    {
        // Singleton pattern untuk memastikan hanya satu AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
        }
        else
        {
            Destroy(gameObject); // Hancurkan duplikasi AudioManager
        }
    }

    // Fungsi untuk mulai memainkan musik
    public void PlayMusic()
    {
        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    // Fungsi untuk menghentikan musik
    public void StopMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
    }
}
