using UnityEngine;

public class FishHook : MonoBehaviour
{
    public Transform hookPoint; // Titik tempat ikan akan menempel
    public Transform initialPosition; // Posisi awal ikan
    public float hookDuration = 10f; // Durasi ikan tertangkap di kail (dalam detik)
    public float fishResistanceStrength = 3f; // Kekuatan perlawanan ikan terhadap tarikannya
    public float pullSpeed = 1f; // Kecepatan ikan bergerak menuju kail
    private bool isFishCaught = false; // Status apakah ikan sudah tertangkap
    private float hookTimer = 0f; // Timer untuk menghitung durasi ikan di kail
    private GameObject caughtFish; // Menyimpan referensi ikan yang tertangkap
    private Rigidbody fishRb; // Referensi Rigidbody ikan untuk manipulasi fisik
    private ScoreManager scoreManager; // Referensi ke ScoreManager

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>(); // Mendapatkan referensi ScoreManager
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish") && !isFishCaught)
        {
            CatchFish(other.gameObject);
        }
    }

    private void Update()
    {
        if (isFishCaught)
        {
            hookTimer += Time.deltaTime;

            // Tarik ikan ke arah kail dengan perlawanan
            FishResistance();

            // Jika waktu tangkapan habis, lepaskan ikan
            if (hookTimer >= hookDuration)
            {
                ResetFish();
            }
        }
    }

    private void CatchFish(GameObject fish)
    {
        if (isFishCaught) return;

        isFishCaught = true;

        fishRb = fish.GetComponent<Rigidbody>();
        if (fishRb != null)
        {
            fishRb.isKinematic = true; // Menonaktifkan fisik ikan agar tidak bergerak bebas
        }

        fish.transform.position = hookPoint.position;
        fish.transform.parent = hookPoint;

        caughtFish = fish;

        Debug.Log("Ikan tertangkap!");
    }

    private void FishResistance()
    {
        if (caughtFish != null)
        {
            Vector3 resistanceDirection = (caughtFish.transform.position - hookPoint.position).normalized;

            // Menarik ikan menjauh dari titik kail dengan resistansi
            caughtFish.transform.position += resistanceDirection * fishResistanceStrength * Time.deltaTime;

            // Menggerakkan ikan secara perlahan menuju titik kail
            caughtFish.transform.position = Vector3.MoveTowards(caughtFish.transform.position, hookPoint.position, pullSpeed * Time.deltaTime);

            // Menambahkan skor berdasarkan kekuatan resistansi ikan
            if (scoreManager != null)
            {
                scoreManager.AddScore((int)(fishResistanceStrength * Time.deltaTime)); // Menambahkan skor secara bertahap berdasarkan resistansi
            }
        }
    }

    private void ResetFish()
    {
        if (caughtFish != null)
        {
            isFishCaught = false;
            hookTimer = 0f;

            caughtFish.transform.parent = null;
            caughtFish.transform.position = initialPosition.position;

            if (fishRb != null)
            {
                fishRb.isKinematic = false;
            }

            Debug.Log("Ikan dilepaskan ke posisi awal.");
            caughtFish = null;
        }
    }
}
