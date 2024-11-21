using UnityEngine;

public class NPCBoatController : MonoBehaviour
{
    public float startDelay = 120f;          // Waktu (dalam detik) sebelum perahu mulai bergerak
    public float moveSpeed = 5f;             // Kecepatan pergerakan perahu
    public Transform startPoint;             // Titik awal perahu
    public Transform targetPoint;            // Titik akhir perahu
    private bool isMoving = false;           // Status apakah perahu mulai bergerak
    private float timer = 0f;                // Timer untuk menghitung waktu yang berlalu

    void Start()
    {
        // Menempatkan perahu pada titik awal
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }

    void Update()
    {
        // Menjalankan timer hingga mencapai waktu yang ditentukan
        if (!isMoving)
        {
            timer += Time.deltaTime;

            // Jika waktu yang ditentukan telah tercapai, perahu mulai bergerak
            if (timer >= startDelay)
            {
                isMoving = true;
            }
        }

        // Jika isMoving aktif, perahu bergerak menuju targetPoint
        if (isMoving && targetPoint != null)
        {
            // Menggerakkan perahu menuju targetPoint dengan kecepatan tertentu
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

            // Memeriksa jika perahu telah mencapai targetPoint
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                isMoving = false; // Menghentikan pergerakan saat mencapai tujuan
                Debug.Log("Perahu telah sampai di titik akhir.");
            }
        }
    }
}
