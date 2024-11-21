using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel; // Panel untuk menu pause
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); // Pastikan panel pause tidak aktif di awal
        Time.timeScale = 1; // Pastikan game berjalan normal di awal
    }

    void Update()
    {
        // Memeriksa input tombol "Escape" atau "P" untuk pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseGame();
        }

        // Melanjutkan game dengan spasi
        if (isPaused && Input.GetKeyDown(KeyCode.Space))
        {
            ResumeGame();
        }
    }

    public void TogglePauseGame()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); // Menampilkan panel pause
        Time.timeScale = 0; // Menghentikan waktu di dalam game
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); // Menyembunyikan panel pause
        Time.timeScale = 1; // Mengembalikan waktu ke normal
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Mengembalikan waktu ke normal sebelum berpindah scene
        SceneManager.LoadScene("intro menu"); // Ganti dengan nama scene menu utama
    }
}
