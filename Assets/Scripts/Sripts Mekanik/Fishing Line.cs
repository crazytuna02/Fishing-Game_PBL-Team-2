using UnityEngine;

public class FishingLine : MonoBehaviour
{
    public LineRenderer lineRenderer; // Referensi ke Line Renderer
    public Transform rodEnd;         // Posisi ujung joran
    public Transform baitEnd;        // Posisi ujung tali pancing
    public int curveResolution = 20; // Jumlah titik pada lengkungan (semakin tinggi, semakin halus)

    public float tensionHeight = 2f; // Tinggi lengkungan saat tali mengendur
    public float smoothSpeed = 5f;  // Kecepatan smoothing untuk transisi lentur
    private Vector3[] linePoints;    // Array untuk menyimpan titik-titik pada tali

    void Start()
    {
        // Inisialisasi array titik
        linePoints = new Vector3[curveResolution];
    }

    void Update()
    {
        DrawLine();
    }

    void DrawLine()
    {
        // Menentukan jumlah titik pada Line Renderer
        lineRenderer.positionCount = curveResolution;

        // Hitung titik-titik pada tali pancing
        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1); // Normalisasi (0 hingga 1)

            // Posisi linear (antara rodEnd dan baitEnd)
            Vector3 targetPos = Vector3.Lerp(rodEnd.position, baitEnd.position, t);

            // Tambahkan tinggi (lengkungan tali)
            float tensionOffset = Mathf.Sin(t * Mathf.PI) * tensionHeight;
            targetPos += Vector3.down * tensionOffset;

            // Smooth damp untuk transisi lentur
            linePoints[i] = Vector3.Lerp(linePoints[i], targetPos, Time.deltaTime * smoothSpeed);

            // Set posisi ke Line Renderer
            lineRenderer.SetPosition(i, linePoints[i]);
        }
    }
}
