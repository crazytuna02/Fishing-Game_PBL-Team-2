using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    [SerializeField] private int scoreValue; // Nilai skor untuk ikan ini
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bait")) // Jika menyentuh umpan
        {
            gameManager.AddScore(scoreValue); // Tambahkan skor sesuai nilai ikan
            Debug.Log("Fish caught! Score: " + scoreValue);
            Destroy(gameObject); // Hancurkan ikan setelah tertangkap
        }
    }
}
