using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class IkanPathfinding : MonoBehaviour
{
    public float radiusPergerakan = 10f;
    public float kecepatan = 5f;
    public float rotasiKecepatan = 10f; // Percepat rotasi ikan
    public float jarakDekatUmpan = 1.5f;
    public float jedaMemakanUmpan = 1f; // Kurangi jeda agar lebih cepat
    public float jedaRespawn = 5f; // Waktu sebelum ikan muncul kembali setelah memakan umpan

    private NavMeshAgent agen;
    private Transform umpan;
    private bool sedangMengejarUmpan = false;
    private static Queue<IkanPathfinding> antreanIkan = new Queue<IkanPathfinding>();
    private static bool umpanSedangDigunakan = false;
    private ScoreManager scoreManager;

    void Start()
    {
        agen = GetComponent<NavMeshAgent>();
        agen.speed = kecepatan;
        umpan = GameObject.FindGameObjectWithTag("Bait")?.transform;

        scoreManager = FindObjectOfType<ScoreManager>();

        // Tambahkan ikan ke antrean pada saat start
        antreanIkan.Enqueue(this);

        StartCoroutine(GiliranIkanSetiap30Detik()); // Tambahkan coroutine untuk pergantian setiap 30 detik
    }

    void Update()
    {
        if (umpan != null && antreanIkan.Count > 0)
        {
            // Hanya ikan yang berada di posisi pertama yang bergerak menuju umpan
            if (!sedangMengejarUmpan && !umpanSedangDigunakan && antreanIkan.Peek() == this)
            {
                agen.SetDestination(umpan.position);
                sedangMengejarUmpan = true;
                umpanSedangDigunakan = true;
            }

            float jarakKeUmpan = Vector3.Distance(transform.position, umpan.position);

            if (sedangMengejarUmpan && jarakKeUmpan < jarakDekatUmpan)
            {
                agen.isStopped = true;
                BeriSkor();

                antreanIkan.Dequeue(); // Hapus ikan dari antrean setelah memakan umpan
                StartCoroutine(RespawnIkan()); // Respawn ikan setelah memakan umpan
            }
        }

        // Rotasi ikan mengikuti arah pergerakannya
        if (agen.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agen.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotasiKecepatan);
        }
    }

    // Coroutine untuk giliran ikan setiap 30 detik
    IEnumerator GiliranIkanSetiap30Detik()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);

            // Pastikan hanya satu ikan yang bergerak menuju umpan pada satu waktu
            if (antreanIkan.Count > 1)
            {
                IkanPathfinding ikanBerikutnya = antreanIkan.Dequeue();  // Ambil ikan pertama
                antreanIkan.Enqueue(ikanBerikutnya);  // Masukkan kembali ke antrean di posisi terakhir
            }
        }
    }

    // Coroutine untuk respawn ikan
    IEnumerator RespawnIkan()
    {
        sedangMengejarUmpan = false;
        umpanSedangDigunakan = false;

        gameObject.SetActive(false); // Hilangkan ikan
        yield return new WaitForSeconds(jedaRespawn); // Tunggu sebelum respawn

        // Pindahkan ikan ke posisi acak
        Vector3 posisiBaru = transform.position + Random.insideUnitSphere * radiusPergerakan;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(posisiBaru, out hit, radiusPergerakan, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }

        gameObject.SetActive(true); // Munculkan kembali ikan
        antreanIkan.Enqueue(this); // Masukkan ikan kembali ke antrean
    }

    // Menambah skor saat ikan memakan umpan
    void BeriSkor()
    {
        if (scoreManager != null)
        {
            if (gameObject.CompareTag("Tuna"))
            {
                scoreManager.AddScore(10);
            }
            else if (gameObject.CompareTag("Bawal")) // Pastikan tag ikan bawal sudah benar
            {
                scoreManager.AddScore(5);
            }
        }
    }
}
