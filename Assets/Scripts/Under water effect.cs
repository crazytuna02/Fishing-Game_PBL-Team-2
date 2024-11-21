using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class UnderwaterEffect : MonoBehaviour
{
    public Material underwaterMaterial;      // Material for underwater effect
    public Texture2D normalMap;              // Normal map for refraction effect
    public float refractionStrength = 0.05f; // Strength of the refraction effect
    public float pixelOffset = 0.01f;        // Offset of pixels for distortion
    public float noiseScale = 1.0f;          // Scale of the noise for the distortion
    public float noiseFrequency = 1.0f;      // Frequency of the noise pattern
    public float noiseSpeed = 1.0f;          // Speed of the noise animation
    public float depthStart = 5.0f;          // Depth at which the effect starts
    public float depthDistance = 10.0f;      // Distance of the depth for the effect

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Ensure underwater material is set
        if (underwaterMaterial == null)
        {
            Debug.LogWarning("Underwater Material is not assigned. Please assign a material.");
        }
    }

    // Called after all rendering is complete to apply post-processing effects
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (underwaterMaterial != null)
        {
            // Set shader properties for underwater effect
            underwaterMaterial.SetFloat("_PixelOffset", pixelOffset);
            underwaterMaterial.SetFloat("_NoiseScale", noiseScale);
            underwaterMaterial.SetFloat("_NoiseFrequency", noiseFrequency);
            underwaterMaterial.SetFloat("_NoiseSpeed", noiseSpeed);
            underwaterMaterial.SetFloat("_DepthStart", depthStart);
            underwaterMaterial.SetFloat("_DepthDistance", depthDistance);

            // Set refraction and normal map properties
            underwaterMaterial.SetTexture("_NormalMap", normalMap);
            underwaterMaterial.SetFloat("_RefractionStrength", refractionStrength);

            // Apply the material effect
            Graphics.Blit(source, destination, underwaterMaterial);
        }
        else
        {
            // Fallback if no material is set
            Graphics.Blit(source, destination);
        }
    }
}
