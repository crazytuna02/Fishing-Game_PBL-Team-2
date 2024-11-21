using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class NewBehaviourScript : MonoBehaviour
{
    // Deklarasi variabel untuk pengaturan fog (kabut)
    public Color fogColor = Color.gray; // Warna default fog
    public float depthStart = 10f;      // Jarak mulai kabut
    public float depthDistance = 50f;   // Jarak maksimum kabut

    // Material untuk efek visual kabut tambahan
    public Material fogMaterial;

    void Start()
    {
        // Mengaktifkan fog pada scene
        RenderSettings.fog = true;

        // Mengatur warna fog dari pengaturan yang telah didefinisikan
        RenderSettings.fogColor = fogColor;

        // Mengatur mode fog menjadi linear agar depthStart dan depthDistance berlaku
        RenderSettings.fogMode = FogMode.Linear;

        // Mengatur jarak awal dan jarak maksimum untuk fog
        RenderSettings.fogStartDistance = depthStart;
        RenderSettings.fogEndDistance = depthDistance;

        // Memastikan material sudah diset
        if (fogMaterial == null)
        {
            Debug.LogWarning("Fog Material tidak ditentukan. Silakan tambahkan material dengan shader khusus.");
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (fogMaterial != null)
        {
            // Set warna dan jarak untuk shader
            fogMaterial.SetColor("_FogColor", fogColor);
            fogMaterial.SetFloat("_DepthStart", depthStart);
            fogMaterial.SetFloat("_DepthDistance", depthDistance);

            // Menggunakan material untuk efek visual kabut
            Graphics.Blit(source, destination, fogMaterial);
        }
        else
        {
            // Jika material belum ada, gunakan fallback tanpa efek tambahan
            Graphics.Blit(source, destination);
        }
    }
}
