using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TunaMovement : MonoBehaviour
{
    public Transform bait; // Posisi umpan atau kail
    public float moveSpeed = 3.5f; // Kecepatan pergerakan ikan
    public float rotationSpeed = 2.0f; // Kecepatan rotasi agar pergerakan halus
    public float detectionRadius = 5.0f; // Radius deteksi umpan
    public float baitTimeLimit = 10.0f; // Waktu maksimum ikan mendekati umpan sebelum bergantian
    private bool isMovingToBait = false;

    private NavMeshAgent agent; // Memerlukan NavMesh untuk pergerakan mulus
    private ScoreManager scoreManager; // Referensi ke ScoreManager

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        
        scoreManager = FindObjectOfType<ScoreManager>(); // Cari ScoreManager di scene
    }

    void Update()
    {
        // Pergerakan ikan secara acak saat tidak mendekati umpan
        if (!isMovingToBait)
        {
            RandomMovement();
            DetectBait();
        }
        else
        {
            MoveToBait();
        }
    }

    void RandomMovement()
    {
        if (agent.remainingDistance < 0.5f)
        {
            Vector3 randomPos = Random.insideUnitSphere * 10;
            randomPos += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, 10, NavMesh.AllAreas);
            agent.SetDestination(hit.position);
        }
    }

    void DetectBait()
    {
        float distanceToBait = Vector3.Distance(transform.position, bait.position);
        if (distanceToBait <= detectionRadius)
        {
            isMovingToBait = true;
        }
    }

    void MoveToBait()
    {
        agent.SetDestination(bait.position);
        agent.speed = moveSpeed * 1.2f; // Sedikit lebih cepat saat mendekati umpan

        // Cek jika ikan tuna mencapai umpan
        if (Vector3.Distance(transform.position, bait.position) < 1.0f) // Jarak sangat dekat dengan umpan
        {
            scoreManager.AddScore(10); // Tambahkan 10 poin untuk ikan tuna
            Debug.Log("Ikan tuna memakan umpan dan mendapatkan 10 poin!");

            // Reset state setelah makan umpan dan langsung bergerak bebas
            isMovingToBait = false;
            agent.SetDestination(transform.position); // Hentikan gerakan menuju umpan
            StartCoroutine(MoveRandomlyAfterEating()); // Pindahkan ikan untuk bergerak bebas setelah makan
        }
    }

    IEnumerator MoveRandomlyAfterEating()
    {
        yield return new WaitForSeconds(1f); // Tunggu sebentar setelah makan umpan
        RandomMovement(); // Lakukan gerakan acak setelah makan umpan
    }
}
