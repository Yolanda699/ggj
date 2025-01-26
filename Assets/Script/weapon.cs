using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class weapon : MonoBehaviour 
{
    public GameObject bullet;
    public Transform[] shootPos;
    private Enemy fatherObj;
    public void SetFather(Enemy obj)
    {
        fatherObj = obj;
    }
    public void Fire()
    {
        for (int i = 0; i < shootPos.Length; i++)
        {
            GameObject obj = Instantiate(bullet, shootPos[i].position, shootPos[i].rotation);
        } 
    }
}
