using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampedCamera : MonoBehaviour
{
    public GameObject clampTo;
    public float xBound;
    public float yBound;
    public float z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, clampTo.transform.position.x - xBound, clampTo.transform.position.x + xBound),
            Mathf.Clamp(transform.position.y, clampTo.transform.position.y - yBound, clampTo.transform.position.y + yBound),
            z);
    }
}
