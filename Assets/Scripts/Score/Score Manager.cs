using UnityEngine;
using TMPro; // Untuk TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;               // TextMeshPro untuk menampilkan skor
    public TextMeshProUGUI timerText;               // TextMeshPro untuk menampilkan sisa waktu
    public TextMeshProUGUI pullStrengthText;        // TextMeshPro untuk menampilkan kekuatan tarikan
    public TextMeshProUGUI finalPullStrengthText;   // TextMeshPro untuk menampilkan kekuatan tarikan terakhir
    public float gameDuration = 60f;                // Durasi permainan dalam detik

    private int totalScore = 0;                     // Total skor akumulasi
    private float timeRemaining;                    // Waktu tersisa
    private bool isGameActive = true;               // Status permainan (berjalan atau selesai)
    private float pullStrength = 0f;                // Nilai kekuatan tarikan saat ini
    private float totalPullStrength = 0f;           // Total akumulasi kekuatan tarikan
    private float lastPullStrength = 0f;            // Kekuatan tarikan terakhir saat menarik ikan

    void Start()
    {
        // Inisialisasi waktu dan mulai hitungan mundur
        timeRemaining = gameDuration;
        UpdateUI(); // Perbarui UI saat permainan dimulai
    }

    void Update()
    {
        if (isGameActive)
        {
            // Kurangi waktu setiap frame
            timeRemaining -= Time.deltaTime;

            // Cegah waktu menjadi negatif
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isGameActive = false;  // Permainan selesai

                // Debug log untuk game over
                Debug.Log("Game Over! Final Score: " + totalScore + ", Final Pull Strength: " + lastPullStrength);

                // Tampilkan nilai final Pull Strength di panel
                finalPullStrengthText.text = "Final Pull Strength: " + lastPullStrength.ToString("F2");
            }
        }

        // Perbarui UI setiap frame
        UpdateUI();
    }

    // Fungsi untuk memperbarui UI
    private void UpdateUI()
    {
        scoreText.text = "Score: " + totalScore.ToString();
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString() + "s";
        pullStrengthText.text = "Pull Strength: " + pullStrength.ToString("F2"); // Format dua angka desimal
    }

    // Fungsi untuk menambah skor hanya jika permainan masih aktif
    public void AddScore(int points)
    {
        if (isGameActive)
        {
            totalScore += points;
            Debug.Log("Score added: " + points + ", Total Score: " + totalScore);
        }
    }

    // Fungsi untuk memperbarui kekuatan tarikan
    public void UpdatePullStrength(float strength)
    {
        if (isGameActive)
        {
            pullStrength = strength;              // Perbarui nilai kekuatan tarikan
            totalPullStrength += strength;        // Akumulasikan kekuatan tarikan
            Debug.Log("Current Pull Strength: " + pullStrength + ", Total Pull Strength: " + totalPullStrength);
        }
    }

    // Fungsi untuk menyimpan kekuatan tarikan terakhir saat ikan tertangkap
    public void RecordLastPullStrength()
    {
        if (isGameActive)
        {
            lastPullStrength = pullStrength; // Simpan nilai tarikan terakhir
            Debug.Log("Last Pull Strength Recorded: " + lastPullStrength);
        }
    }

    // Fungsi untuk mengambil total kekuatan tarikan
    public float GetTotalPullStrength()
    {
        return totalPullStrength; // Kembalikan total akumulasi kekuatan tarikan
    }
}
