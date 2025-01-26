using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Fire Settings")]
    private float fireOffsetTime = 1f; // Time between shots
    private float nowTime = 0;
    public Transform[] shootPos; // Positions where bullets are generated
    public GameObject bulletObject; // Bullet prefab

    private bool fireState = false; // Determines if the enemy is firing

    [Header("Audio Settings")]
    public AudioSource fireAudioSource; // AudioSource to play when firing

    private void Update()
    {
        nowTime += Time.deltaTime;

        if (nowTime > fireOffsetTime)
        {
            if (fireState == true)
            {
                Fire();
                nowTime = 0;
            }
        }
    }

    public void Fire()
    {
        // Play fire sound
        if (fireAudioSource != null && !fireAudioSource.isPlaying)
        {
            fireAudioSource.Play();
        }

        // Instantiate bullets at each shoot position
        for (int i = 0; i < shootPos.Length; i++)
        {
            GameObject obj = Instantiate(bulletObject, shootPos[i].position, shootPos[i].rotation);
            BulletObject bullet = obj.GetComponent<BulletObject>();

            // Set the bullet size
            bullet.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }

    public void ActivateFire()
    {
        fireState = true;
    }
}
