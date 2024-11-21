using UnityEngine;

public class PasangUmpan : MonoBehaviour
{
    public Transform hook; // Drag objek hook di sini
    public string pushButtonInput = "Fire1"; // Ganti dengan nama input untuk push button di controller
    public Transform initialPosition; // Posisi awal umpan
    public float baitDuration = 15f; // Durasi umpan bertahan di kail setelah dimakan ikan

    private bool isBaitAttached = false; // Flag untuk mengecek apakah umpan terpasang
    private bool isBaitEaten = false; // Flag untuk mengecek apakah umpan dimakan ikan
    private float baitTimer = 0f; // Timer untuk menghitung durasi umpan di kail

    void Start()
    {
        // Menyimpan posisi awal umpan
        if (initialPosition == null)
        {
            initialPosition = transform; // Gunakan posisi objek ini sebagai posisi awal jika belum diatur
        }
    }

    void Update()
    {
        // Input dari keyboard (tombol D)
        if (Input.GetKeyDown(KeyCode.D))
        {
            AttachBait();
        }

        // Input dari push button pada controller
        if (Input.GetButtonDown(pushButtonInput))
        {
            AttachBait();
        }

        // Hitung waktu umpan berada di kail jika sudah dimakan
        if (isBaitEaten)
        {
            baitTimer += Time.deltaTime; // Mulai hitung waktu
            Debug.Log($"Bait Timer: {baitTimer:F2}"); // Debugging untuk memonitor waktu

            if (baitTimer >= baitDuration)
            {
                ResetBaitPosition(); // Reset umpan ke posisi awal setelah waktu habis
            }
        }
    }

    public void AttachBait()
    {
        if (!isBaitAttached)
        {
            // Pindahkan umpan ke hook
            transform.position = hook.position;

            // Buat umpan mengikuti hook
            transform.parent = hook;

            isBaitAttached = true;
            isBaitEaten = false; // Pastikan umpan tidak dianggap dimakan sebelum waktunya
            Debug.Log("Umpan berhasil dipasang.");
        }
    }

    // Panggil metode ini dari script lain (misalnya dari FishMovement saat ikan makan umpan)
    public void BaitEaten()
    {
        if (isBaitAttached && !isBaitEaten)
        {
            isBaitEaten = true; // Umpan dimakan ikan
            baitTimer = 0f; // Reset timer
            Debug.Log("Umpan dimakan ikan!");
        }
    }

    public void ResetBaitPosition()
    {
        // Reset hanya jika umpan sedang di kail atau dimakan
        if (isBaitAttached || isBaitEaten)
        {
            // Reset umpan ke posisi awal
            transform.position = initialPosition.position;
            transform.parent = null; // Lepaskan umpan dari hook

            isBaitAttached = false;
            isBaitEaten = false;
            baitTimer = 0f; // Reset timer
            Debug.Log("Umpan dikembalikan ke posisi awal.");
        }
    }
}
