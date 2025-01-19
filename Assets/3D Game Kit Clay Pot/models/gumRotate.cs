using UnityEngine;

public class gumRotate : MonoBehaviour
{
    GameObject theGum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        theGum = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        theGum.transform.Rotate(0, 0.2f, 0);
    }
}
