using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    // Start is called before the first frame update

    UIManager uIManager;

    void Start()
    {
        uIManager = UIManager.FindFirstObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Goomba>() != null)
        {
            uIManager.win();
        }
    }
}
