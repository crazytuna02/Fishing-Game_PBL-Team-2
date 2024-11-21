using UnityEngine;
using UnityEngine.AI;

public class FishMovement : MonoBehaviour
{
    public Transform bait; // Posisi umpan
    public Transform initialPosition; // Posisi awal ikan
    public Transform rod; // Referensi untuk posisi joran yang bergerak
    public float moveSpeed = 3.5f; // Kecepatan pergerakan ikan
    public float rotationSpeed = 2.0f; // Kecepatan rotasi untuk pergerakan yang halus
    public float detectionRadius = 5.0f; // Radius deteksi umpan
    public float radiusPergerakan = 10f; // Radius pergerakan acak
    public float rodPullSpeed = 5f; // Kecepatan pergerakan ikan mengikuti joran
    public float fishPullStrength = 3f; // Kekuatan perlawanan ikan saat melawan
    public float hookWaitDuration = 15f; // Durasi ikan menunggu di umpan sebelum dilepaskan
    public float resistanceStrengthMultiplier = 2f; // Penguat perlawanan ikan saat ditarik

    private NavMeshAgent agent; // Referensi ke NavMeshAgent
    private bool isMovingToBait = false; // Flag untuk memeriksa apakah ikan bergerak ke umpan
    private bool isHooked = false; // Flag untuk memeriksa apakah ikan sudah terjerat kail
    private bool isResisting = false; // Flag untuk perlawanan ikan saat melawan tarikan joran
    private float hookTimer = 0f; // Timer untuk durasi ikan di kail
    private ScoreManager scoreManager; // Referensi ke ScoreManager
    private PasangUmpan pasangUmpan; // Referensi ke script PasangUmpan

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        scoreManager = FindObjectOfType<ScoreManager>(); // Mencari ScoreManager di scene
        pasangUmpan = FindObjectOfType<PasangUmpan>(); // Mencari script PasangUmpan di scene
    }

    void Update()
    {
        if (isHooked)
        {
            hookTimer += Time.deltaTime;

            // Perlawanan ikan saat di kail
            FishResistance();

            if (hookTimer >= hookWaitDuration)
            {
                ResetFishAndBait(); // Reset ikan dan umpan setelah waktu habis
            }
        }
        else
        {
            if (!isMovingToBait)
            {
                RandomMovement(); // Ikan bergerak secara acak
                DetectBait(); // Deteksi apakah ikan mendekati umpan
            }
            else
            {
                MoveToBait(); // Ikan bergerak menuju umpan
            }
        }

        // Rotasi ikan agar menghadap arah pergerakannya
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void RandomMovement()
    {
        if (agent.remainingDistance < 0.5f)
        {
            Vector3 randomPos = Random.insideUnitSphere * radiusPergerakan;
            randomPos += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, radiusPergerakan, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
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

        // Cek jika ikan mencapai umpan
        if (Vector3.Distance(transform.position, bait.position) < 1.0f)
        {
            isHooked = true; // Ikan terjerat
            hookTimer = 0f; // Reset timer

            // Matikan NavMeshAgent untuk menghentikan ikan
            agent.ResetPath();
            agent.enabled = false;

            // Tempelkan ikan ke kail
            transform.position = bait.position; // Atur posisi ikan ke umpan
            transform.parent = bait; // Kaitkan ikan dengan umpan

            // Tambah skor berdasarkan jenis ikan
            scoreManager.AddScore(gameObject.CompareTag("Tuna") ? 10 : 5);

            Debug.Log(gameObject.CompareTag("Tuna") ? "Tuna memakan umpan!" : "Bawal memakan umpan!");

            isMovingToBait = false; // Reset flag pergerakan
        }
    }

    void FishResistance()
    {
        if (!isResisting)
        {
            isResisting = true; // Ikan mulai melawan tarikan

            // Menghitung arah perlawanan ikan
            Vector3 resistanceDirection = (transform.position - rod.position).normalized;

            // Menentukan arah tarik ikan ke kail
            Vector3 pullDirection = rod.position - transform.position;

            // Menambahkan gaya perlawanan yang lebih besar
            float resistanceMultiplier = Mathf.Lerp(1f, resistanceStrengthMultiplier, Vector3.Distance(transform.position, rod.position) / 10f); // Penguatan perlawanan

            // Tarik-menarik antara pemain dan ikan
            transform.position += resistanceDirection * fishPullStrength * resistanceMultiplier * Time.deltaTime; // Ikan melawan tarikannya

            // Ikan mencoba bergerak menuju kail
            transform.position = Vector3.MoveTowards(transform.position, rod.position, rodPullSpeed * Time.deltaTime);

            // Jika ikan sudah dekat dengan kail, pastikan ia tidak melewati posisi kail
            float distanceToRod = Vector3.Distance(transform.position, rod.position);
            if (distanceToRod < 0.5f)
            {
                transform.position = rod.position; // Tempelkan ikan ke kail jika terlalu dekat
                isResisting = false; // Ikan tidak melawan lagi setelah sampai ke kail
            }
        }
    }

    void ResetFishAndBait()
    {
        isHooked = false; // Lepaskan ikan dari kail
        hookTimer = 0f;

        // Reset posisi ikan ke posisi awal
        transform.position = initialPosition.position;
        transform.parent = null; // Lepaskan hubungan dengan kail

        // Aktifkan kembali NavMeshAgent
        agent.enabled = true;

        // Reset umpan ke posisi awal
        pasangUmpan.ResetBaitPosition();

        // Aktifkan kembali gerakan acak ikan
        RandomMovement();
    }
}
