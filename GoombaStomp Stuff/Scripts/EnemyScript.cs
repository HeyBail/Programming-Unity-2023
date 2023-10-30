using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : Respawnable
{
    private Animator animator;
    private Rigidbody2D rigidBody;
    private PlayerController player;

    public int agroRange = 5;

    bool alive = true;
    // Start is called before the first frame update
    void Start()
    {
        initialize();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = PlayerController.FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive && Vector2.Distance(player.transform.position, transform.position) < agroRange) // check if angry, or if any friends have died
        {
            rigidBody.AddForce((player.transform.position - transform.position).normalized * Vector2.right * 2 * rigidBody.mass); // maybe some ray casting idk
            animator.SetBool("run", true);

            if (player.transform.position.x - transform.position.x < .5) GetComponent<SpriteRenderer>().flipX = false;
            else if (player.transform.position.x - transform.position.x > .5) GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            animator.SetBool("run", false);

            if (rigidBody.velocity.x < .5) GetComponent<SpriteRenderer>().flipX = false;
            else if (rigidBody.velocity.x > .5) GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alive)
        {
            bool bonked = false;
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y < 0) //this gets the normal from the other collision, aka the hammer, maybe do negative y
                {
                    bonked = true;

                    //Debug.Log(p.point.y);
                }
            }

            PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
            if (player != null && !bonked && alive)
            {
                Debug.Log("boink, you dead");
                player.hit(rigidBody.velocity); // probably have to fix / change this
            }
            else if (player != null && bonked && alive)
            {
                Debug.Log("boink, *screams in agony*");
                alive = false;
                Dies();
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
        animator.SetBool("alive", alive);
        //GetComponent<PolygonCollider2D>().excludeLayers = ~(1 << 3);

        //stop colliding with the player
        StartCoroutine(sCream()); // fix :?
    }
    IEnumerator sCream() 
    {
        animator.SetBool("alive", alive);

        //set collider to trigger

        yield return new WaitForSeconds(3);

        if (respawnable)
        {
            Respawn();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
