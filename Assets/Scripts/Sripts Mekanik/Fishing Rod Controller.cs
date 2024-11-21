using UnityEngine;
using System.Collections.Generic;

public class FishingRodController : MonoBehaviour
{
    public Transform rod;                     // Referensi objek joran
    public List<Transform> fishes;            // Referensi daftar objek ikan
    public float liftSpeed = 1f;              // Kecepatan mengangkat joran
    public float maxLiftHeight = 2f;          // Ketinggian maksimum angkatan joran
    public float bendAmount = 20f;            // Jumlah kelengkungan saat joran diangkat
    public float lowerSpeed = 0.5f;           // Kecepatan menurunkan joran secara otomatis
    public float fishPullStrength = 1f;       // Kekuatan tarikan ikan
    public float playerPullStrength = 1f;     // Kekuatan tarikan pemain
    public float pullIncrement = 0.2f;        // Inkrement kekuatan setiap tarikan
    public float pullCooldown = 1f;           // Cooldown untuk menambah kekuatan
    public float horizontalLimit = 2f;        // Batas horizontal joran (kiri-kanan)

    public ScoreManager scoreManager;         // Referensi ke ScoreManager

    private float currentLift = 0f;           // Ketinggian angkatan joran saat ini
    private Vector3 initialPosition;          // Posisi awal joran
    private Quaternion initialRotation;       // Rotasi awal joran
    private bool isFishHooked = false;        // Flag untuk mengecek apakah ada ikan yang terjerat
    private float lastPullTime = 0f;          // Waktu terakhir tarikan pemain

    void Start()
    {
        // Menyimpan posisi dan rotasi awal joran
        initialPosition = rod.localPosition;
        initialRotation = rod.localRotation;
    }

    void Update()
    {
        if (isFishHooked)
        {
            HandleRodPulling(); // Pergerakan joran saat ikan terjerat
        }
        else
        {
            HandleRodMovement(); // Pergerakan joran biasa
        }

        // Menambah kekuatan tarikan pemain
        if (Input.GetKey(KeyCode.Space) && Time.time - lastPullTime >= pullCooldown)
        {
            lastPullTime = Time.time; // Memperbarui waktu terakhir tarikan
            IncreasePlayerPullStrength(); // Meningkatkan kekuatan tarikan
        }

        UpdateRodPositionAndRotation(); // Perbarui posisi dan rotasi joran
    }

    private void HandleRodMovement()
    {
        // Mengangkat joran (tekan tombol S)
        if (Input.GetKey(KeyCode.S))
        {
            currentLift += liftSpeed * Time.deltaTime;
        }
        // Menurunkan joran (tekan tombol W)
        else if (Input.GetKey(KeyCode.W))
        {
            currentLift -= liftSpeed * Time.deltaTime;
        }
        else
        {
            currentLift -= lowerSpeed * Time.deltaTime; // Turun perlahan jika tidak ada input
        }

        // Membatasi ketinggian joran
        currentLift = Mathf.Clamp(currentLift, 0, maxLiftHeight);

        // Menggerakkan joran ke kiri (tekan tombol A)
        if (Input.GetKey(KeyCode.A))
        {
            rod.localPosition += Vector3.left * liftSpeed * Time.deltaTime;
        }
        // Menggerakkan joran ke kanan (tekan tombol D)
        else if (Input.GetKey(KeyCode.D))
        {
            rod.localPosition += Vector3.right * liftSpeed * Time.deltaTime;
        }

        // Membatasi posisi horizontal joran
        float clampedX = Mathf.Clamp(rod.localPosition.x, -horizontalLimit, horizontalLimit);
        rod.localPosition = new Vector3(clampedX, rod.localPosition.y, rod.localPosition.z);
    }

    private void HandleRodPulling()
    {
        foreach (var fish in fishes)
        {
            if (fish != null)
            {
                float distanceToFish = Vector3.Distance(rod.position, fish.position);

                // Tarikan ikan terhadap joran
                if (distanceToFish < 5f) // Jika ikan cukup dekat
                {
                    currentLift += fishPullStrength * Time.deltaTime;
                }
                else
                {
                    currentLift -= lowerSpeed * Time.deltaTime; // Turun perlahan
                }

                currentLift = Mathf.Clamp(currentLift, 0, maxLiftHeight); // Batasi ketinggian
            }
        }
    }

    private void UpdateRodPositionAndRotation()
    {
        rod.localPosition = initialPosition + new Vector3(0, currentLift, 0);
        rod.localRotation = initialRotation * Quaternion.Euler(0, 0, -currentLift * bendAmount);
    }

    private void IncreasePlayerPullStrength()
    {
        playerPullStrength += pullIncrement; // Tambah kekuatan tarikan
        if (scoreManager != null)
        {
            scoreManager.UpdatePullStrength(playerPullStrength); // Update ke ScoreManager
        }
        Debug.Log("Player Pull Strength: " + playerPullStrength);
    }

    public void HookFish()
    {
        isFishHooked = true;
    }

    public void UnhookFish()
    {
        isFishHooked = false;
    }

    public void AddFish(Transform newFish)
    {
        if (!fishes.Contains(newFish))
        {
            fishes.Add(newFish);
        }
    }

    public void RemoveFish(Transform fishToRemove)
    {
        if (fishes.Contains(fishToRemove))
        {
            fishes.Remove(fishToRemove);
        }
    }

    public void CatchFish()
    {
        if (isFishHooked && scoreManager != null)
        {
            scoreManager.AddScore(10); // Tambah skor
            scoreManager.RecordLastPullStrength(); // Simpan nilai pull strength terakhir
            UnhookFish(); // Lepas ikan
        }
    }
}
