using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpParticles : MonoBehaviour
{
    public GameObject particlePrefab;
    public float yOffset = 0.1f;

    public void PlayJumpEffect()
    {
        if (particlePrefab == null) return;

        Vector3 spawnPosition = transform.position + Vector3.down * yOffset;
        GameObject effect = Instantiate(particlePrefab, spawnPosition, Quaternion.identity);
        Destroy(effect, 1f);
    }
}
