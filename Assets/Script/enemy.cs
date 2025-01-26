using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private float fireOffsetTime = 1f;
    private float nowTime = 0;
    public Transform[] shootPos;
    public GameObject bulletObject;

    bool fireState = false;

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
        for (int i = 0; i < shootPos.Length; i++)
        {
            GameObject obj = Instantiate(bulletObject, shootPos[i].position, shootPos[i].rotation);
            BulletObject bullet = obj.GetComponent<BulletObject>();
          
            bullet.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        }
    }

    public void ActivateFire()
    {
        fireState = true;
    }
}
