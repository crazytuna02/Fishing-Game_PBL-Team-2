using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Untuk navigasi UI menggunakan EventSystem
using TMPro; // TextMeshPro untuk UI
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float gameDuration = 60f; // Durasi game (1 menit)
    [SerializeField] private GameObject scorePanel; // Panel skor
    [SerializeField] private TextMeshProUGUI scoreText; // Menggunakan TextMeshProUGUI untuk teks skor
    [SerializeField] private Button playButton; // Tombol Play
    [SerializeField] private Button exitButton; // Tombol Exit
    private float timer;
    private int playerScore;
    private bool isGameOver = false;

    private GameObject currentButton; // Tombol yang dipilih saat ini

    void Start()
    {
        timer = gameDuration; // Set timer ke durasi yang diinginkan
        scorePanel.SetActive(false); // Sembunyikan panel di awal
        Time.timeScale = 1; // Pastikan game berjalan normal di awal

        // Atur tombol awal untuk navigasi
        if (playButton != null)
        {
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
            currentButton = playButton.gameObject;
        }
    }

    void Update()
    {
        if (!isGameOver) // Jika game belum selesai
        {
            timer -= Time.deltaTime; // Kurangi timer setiap frame

            if (timer <= 0) // Jika waktu habis
            {
                timer = 0; // Pastikan timer tidak kurang dari 0
                EndGame(); // Panggil fungsi EndGame
            }
        }
        else
        {
            HandleNavigation(); // Navigasi tombol saat game over
        }
    }

    public void AddScore(int amount) // Fungsi menambah skor
    {
        if (!isGameOver) // Tambah skor hanya jika game belum selesai
        {
            playerScore += amount;
        }
    }

    void EndGame() // Fungsi akhir permainan
    {
        isGameOver = true;
        scorePanel.SetActive(true); // Tampilkan panel skor
        scoreText.text = "Skor Akhir: " + playerScore; // Tampilkan skor akhir menggunakan TextMeshPro
        Time.timeScale = 0; // Hentikan semua pergerakan game

        // Atur tombol awal untuk navigasi
        if (playButton != null)
        {
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
            currentButton = playButton.gameObject;
        }
    }

    private void HandleNavigation()
    {
        // Navigasi kiri (tombol A atau joystick axis)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal") < -0.5f)
        {
            if (currentButton == exitButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(playButton.gameObject);
                currentButton = playButton.gameObject;
            }
        }

        // Navigasi kanan (tombol D atau joystick axis)
        if (Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal") > 0.5f)
        {
            if (currentButton == playButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(exitButton.gameObject);
                currentButton = exitButton.gameObject;
            }
        }

        // Konfirmasi pilihan (tombol Space atau joystick button)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            selectedButton?.onClick.Invoke();
        }
    }

    public void RestartGame() // Fungsi untuk restart game
    {
        Time.timeScale = 1; // Kembalikan game ke kondisi berjalan normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Muat ulang scene saat ini
    }

    public void PlayGame() // Fungsi untuk tombol Play (mulai ulang permainan)
    {
        Time.timeScale = 1; // Pastikan game berjalan normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Muat ulang scene
    }

    public void ExitGame() // Fungsi untuk tombol Exit (kembali ke menu utama)
    {
        Time.timeScale = 1; // Pastikan waktu berjalan normal
        SceneManager.LoadScene("intro menu"); // Ganti dengan nama scene menu utama Anda
    }
}
