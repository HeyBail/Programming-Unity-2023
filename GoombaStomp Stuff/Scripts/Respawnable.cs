using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Respawnable : MonoBehaviour
{
    private Vector3 initialPosition;
    public GameObject respawnObject;
    public bool respawnable = false;

    // Start is called before the first frame update
    public void initialize()
    {
        initialPosition = transform.position;
    }
    protected void Respawn() 
    {
        GameObject.Instantiate(respawnObject, initialPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
