using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossScript : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidBody;

    private bool alive= true;
    private bool throwing = false;
    private bool isHit = false;

    private int coolDown = 3;
    private float lastAttack;
    public int healthPoints;

    public GameObject projectile;    
    private Goomba goomba;

    

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        healthPoints = 3;
        rigidBody = GetComponent<Rigidbody2D>();
        lastAttack = Time.time;
        goomba = Goomba.FindFirstObjectByType<Goomba>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive) 
        {
            if (isHit)
            {
                if (transform.position.y < 38) // hardcoded because im lazy, when it lands
                {
                    if (healthPoints == 0)
                    {
                        alive = false;
                        animator.SetBool("alive", alive);
                    }
                    else
                    {
                        isHit = false;
                        animator.SetBool("hit", isHit);
                        lastAttack = Time.time;
                    }
                }
            }
            else if (!throwing)
            {
                if ((goomba.transform.position - transform.position).magnitude < 15)
                {
                    if (Time.time - lastAttack > coolDown)
                    {
                        StartCoroutine(attack());
                    }
                }
            }
        }
    }

    IEnumerator hit()
    {
        animator.SetBool("hit", true);
        rigidBody.AddForce(new Vector2(-110,500) * rigidBody.mass);

        yield return new WaitForSeconds(.2f);
        isHit = true;


        throwing = false;
        animator.SetBool("throwing",throwing);

        healthPoints--;

        

        switch (healthPoints)
        {
            case 2:
                //move to location or set next destination
                
                break;
            case 1:
                //move to location or set next destination

                break;
            case 0:
                //move to location or set next destination
                break;
                }
    }

    private void die() 
    { 
    
    }

    IEnumerator attack() //make this an enum
    {
        //play animation
        throwing = true;
        animator.SetBool("throwing", throwing);

        yield return new WaitForSeconds(.6f);
        //at end of animation (probably just a wait thing)
        //create projectile prefab at location with velocity
        if (throwing)
        { 
            GameObject.Instantiate(projectile, transform.position + new Vector3(8, -5, 0), Quaternion.identity);
            throwing = false;
            animator.SetBool("throwing", throwing);
            lastAttack = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alive)
        {
            Debug.Log("TRIGGERED");

            PlayerController playerController = collision.GetComponentInParent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("Player ran into boss");
                playerController.hit(new Vector2(0, 1));
            }
            else if (collision.gameObject.GetComponent<BossProjectile>() != null)
            {
                Debug.Log("Boss Got Hit");
                StartCoroutine(hit());
                Destroy(collision.gameObject);
            }
        }
    }
}
