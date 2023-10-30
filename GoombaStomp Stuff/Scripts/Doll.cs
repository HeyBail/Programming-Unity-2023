using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doll : Respawnable
{
    private Rigidbody2D rigidBody;
    private PlayerController player;

    bool alive;

    public Sprite deadSprite;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = PlayerController.FindAnyObjectByType<PlayerController>();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidBody.velocity.x < .5) GetComponent<SpriteRenderer>().flipX = false;
        else if (rigidBody.velocity.x > .5) GetComponent<SpriteRenderer>().flipX = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alive)
        {
            bool bonked = false;
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y < 0)
                {
                    bonked = true;

                    //Debug.Log(p.point.y);
                }
            }

            if (player != null && bonked)
            {
                Debug.Log("boink, *screams in agony*");
                GetComponent<SpriteRenderer>().sprite = deadSprite;
                alive = false;
                Dies(); // just make this a coroutine and call this
            }
        }
        else 
        {
            if (collision.gameObject.tag == "Player")
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!alive)
        {
            if (collision.gameObject.tag == "Player")
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
            }
        }
    }

    private void Dies()
    {
        //GetComponent<PolygonCollider2D>().excludeLayers = ~(1 << 3);

        //stop colliding with the player

        StartCoroutine(sCream());
    }
    IEnumerator sCream()
    {
        //set collider to trigger
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
