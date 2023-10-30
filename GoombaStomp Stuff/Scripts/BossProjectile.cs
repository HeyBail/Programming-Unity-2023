using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(new Vector2(500, 100));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if goommba
        //knockback
        PlayerController playerController = collision.gameObject.GetComponentInParent<PlayerController>();

        Goomba goombaScript = collision.gameObject.GetComponent<Goomba>();
        Hammer hammerScript = collision.gameObject.GetComponent<Hammer>();

        if (goombaScript != null)
        {
            playerController.hit(new Vector2(0, 1));
            Destroy(gameObject);
        }
        else if (hammerScript != null)
        {
            rigidBody.AddForce(-rigidBody.totalForce + new Vector2(-500, 250)); // could just make it constant instead
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
