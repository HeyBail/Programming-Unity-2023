using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private GameManager gameManager;
    public int nextLevel; 



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.FindFirstObjectByType<GameManager>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerController>() != null) 
        {
            gameManager.LoadLevel(nextLevel);
        }
    }
}
