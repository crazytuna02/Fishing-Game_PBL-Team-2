using UnityEngine;
using UnityEngine.UI;       // Untuk mengelola UI seperti tombol dan panel
using UnityEngine.SceneManagement;  // Untuk manajemen perpindahan antar scene

public class menu : MonoBehaviour
{
    // Referensi ke panel UI
    public GameObject menuPanel;     // Panel utama
    public GameObject infoPanel;     // Panel info

    // Fungsi yang dijalankan saat aplikasi dimulai
    void Start()
    {
        // Menampilkan menu utama dan menyembunyikan info panel saat aplikasi dimulai
        menuPanel.SetActive(true);
        infoPanel.SetActive(false);
    }

    // Fungsi untuk tombol start, memuat scene berdasarkan string yang dimasukkan
    public void StartButton(string sceneName)
    {
        // Memuat scene yang diinginkan berdasarkan nama yang diberikan
        SceneManager.LoadScene(sceneName);
    }

    // Fungsi untuk menampilkan panel info
    public void InfoButton()
    {
        // Menyembunyikan menu utama dan menampilkan info panel
        menuPanel.SetActive(false);
        infoPanel.SetActive(true);
    }

    // Fungsi untuk kembali dari panel info ke menu utama
    public void BackButton()
    {
        // Menampilkan menu utama dan menyembunyikan info panel
        menuPanel.SetActive(true);
        infoPanel.SetActive(false);
    }
}
