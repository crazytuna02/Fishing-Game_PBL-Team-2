using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishEnemyManager : MonoBehaviour
{
    public List<StarfishEnemy> starfishEnemies; // List untuk semua objek bintang laut
    public Transform targetPoint;               // Titik tujuan yang dituju bintang laut
    public float jumpHeight = 2f;               // Tinggi lompatan
    public float jumpSpeed = 5f;                // Kecepatan lompatan
    public float jumpInterval = 1f;             // Interval antar lompatan

    private int currentStarfishIndex = 0;       // Indeks bintang laut yang akan lompat berikutnya

    void Start()
    {
        // Memulai lompatan bintang laut pertama
        StartCoroutine(JumpToTargetSequentially());
    }

    IEnumerator JumpToTargetSequentially()
    {
        while (true)
        {
            // Dapatkan bintang laut berikutnya berdasarkan indeks
            StarfishEnemy currentStarfish = starfishEnemies[currentStarfishIndex];

            // Aktifkan bintang laut saat mulai lompatan
            currentStarfish.gameObject.SetActive(true);

            // Lakukan lompatan bintang laut ke target
            yield return StartCoroutine(currentStarfish.JumpToTarget(targetPoint, jumpHeight, jumpSpeed));

            // Nonaktifkan bintang laut setelah mencapai target
            currentStarfish.gameObject.SetActive(false);

            // Update indeks untuk bintang laut berikutnya (loop kembali ke awal jika sudah mencapai akhir list)
            currentStarfishIndex = (currentStarfishIndex + 1) % starfishEnemies.Count;

            // Jeda sebelum bintang laut berikutnya meloncat
            yield return new WaitForSeconds(jumpInterval);
        }
    }
}
