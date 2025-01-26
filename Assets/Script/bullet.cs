using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class BulletObject : MonoBehaviour 
{
    private float moveSpeed = 5;
    private enemy fatherObj;
    
    private void Start()
    {
        
    }
    private void Update()
    {
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
    private void Rotation(GameObject bullet)
    {
        bullet.transform.Rotate(0, 0, 90);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
        
    }
}
