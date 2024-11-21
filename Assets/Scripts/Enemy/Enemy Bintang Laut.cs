using System.Collections;
using UnityEngine;

public class StarfishEnemy : MonoBehaviour
{
    // Fungsi coroutine untuk lompatan ke target
    public IEnumerator JumpToTarget(Transform targetPoint, float jumpHeight, float jumpSpeed)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(targetPoint.position.x, startPosition.y, targetPoint.position.z);
        float jumpProgress = 0;

        // Loop untuk animasi lompatan
        while (jumpProgress < 1)
        {
            jumpProgress += Time.deltaTime * jumpSpeed;

            // Perhitungan ketinggian melengkung (parabola)
            float height = Mathf.Sin(Mathf.PI * jumpProgress) * jumpHeight;

            // Update posisi bintang laut
            transform.position = Vector3.Lerp(startPosition, targetPosition, jumpProgress) + Vector3.up * height;

            yield return null;
        }

        // Memastikan posisi bintang laut di titik tujuan setelah lompatan selesai
        transform.position = targetPosition;
    }
}
