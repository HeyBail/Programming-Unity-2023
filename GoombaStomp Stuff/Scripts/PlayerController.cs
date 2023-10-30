using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject hammer;
    private Vector2 pullForce;
    
    
    public GameObject intention;


    public GameObject goomba;
    private Rigidbody2D goombaBody;


    private float mouseSpeed = 25000;

    private float hammerForce = 50000;

    private Vector2 mouseForce;


    private DistanceJoint2D intentionJoint;
    private TargetJoint2D hammerToIntention;

    // Start is called before the first frame update
    void Start()
    {
        hammerToIntention = hammer.GetComponent<TargetJoint2D>();
        pullForce = hammerToIntention.reactionForce;

        intentionJoint = intention.GetComponent<DistanceJoint2D>();

        goombaBody = goomba.GetComponent<Rigidbody2D>();
    }


    private void OnLook(InputValue value)
    {
        mouseForce = Vector2.ClampMagnitude(value.Get<Vector2>(), 50f); ;
        if (mouseForce != Vector2.zero)
        {
            intention.GetComponent<TargetJoint2D>().target = intention.transform.position;
            intention.GetComponent<Rigidbody2D>().AddForce(mouseForce * Time.deltaTime * mouseSpeed);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    { 
        Vector2 gravityForce = Vector2.zero;
        ContactPoint2D contact = collision.GetContact(0);
        if (contact.normal.y > 0)
        {
            //float hammerAngleGravityFactor = 1 - (.01f * (Vector2.up.y - (hammer.transform.position - goomba.transform.position).normalized.y));
            //gravityForce = new Vector2(0, (-Physics2D.gravity.y * goombaBody.gravityScale * goombaBody.mass) * hammerAngleGravityFactor);
            gravityForce = new Vector2(0, (-Physics2D.gravity.y * goombaBody.gravityScale * goombaBody.mass)) * .9f;
        }


        if (Vector2.Distance(pullForce, hammerToIntention.reactionForce) * Time.deltaTime < .1f)
        {
            float dampenerConstant = 100000;
            //Debug.Log(-(goombaBody.velocity.normalized) * goombaBody.velocity.magnitude * Time.deltaTime * dampenerConstant);
            goombaBody.AddForce(-(goombaBody.velocity.normalized) * goombaBody.velocity.magnitude * Time.deltaTime * dampenerConstant);
        }

        bool bonk = false;
        ContactPoint2D bonkPosition = collision.contacts[0];
        foreach (ContactPoint2D point in collision.contacts)
        {
            if (point.normal.y < 0)
            {
                bonk = true;
                bonkPosition = point;
            }
        }

        BrickHandler bh = collision.gameObject.GetComponent<BrickHandler>();
        if (bonk && bh != null) 
        {
            bh.HandleBonk(bonkPosition.point + new Vector2(0, 0.55f)); // probably have to adjust the value
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 force = Time.deltaTime * -(mouseForce) * hammerForce;
        force = Vector2.ClampMagnitude(force + force.normalized * 2000, 10000f);

        ContactPoint2D contact = collision.GetContact(0);

        //we need to get a fraction form the difference and apply it to the magnitude
        float surfaceNormalFactor = 1 - (Mathf.Clamp(Vector2.Distance(collision.GetContact(0).normal, force.normalized),0 , Mathf.Sqrt(2)) / Mathf.Sqrt(2));

        Vector2 normalOffsetFactor = ((5 * force.normalized) + contact.normal).normalized;

        force = force.magnitude * surfaceNormalFactor * normalOffsetFactor;

        Vector2 gravityForce = Vector2.zero;
        if (contact.normal.y > 0)
        {
            //float hammerAngleGravityFactor = 1 - (.01f * (Vector2.up.y - (hammer.transform.position - goomba.transform.position).normalized.y));
            //gravityForce = new Vector2(0, (-Physics2D.gravity.y * goombaBody.gravityScale * goombaBody.mass) * hammerAngleGravityFactor);
            gravityForce = new Vector2(0, (-Physics2D.gravity.y * goombaBody.gravityScale * goombaBody.mass)) * .9f;
        }
        
        goombaBody.AddForce(force + gravityForce); //add the force of gravity from the Goomba or something?, probably move to update

        if (Vector2.Distance(pullForce, hammerToIntention.reactionForce) * Time.deltaTime < .1f)
        {
            float dampenerConstant = 100000;
            //Debug.Log(-(goombaBody.velocity.normalized) * goombaBody.velocity.magnitude * Time.deltaTime * dampenerConstant);
            goombaBody.AddForce(-(goombaBody.velocity.normalized) * goombaBody.velocity.magnitude * Time.deltaTime * dampenerConstant);
        }

        foreach (var item in collision.contacts)
        {
            Debug.DrawRay(item.point, force * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 5f);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Vector2 gravityForce = Vector2.zero;
        ContactPoint2D contact = collision.GetContact(0);
        if (contact.normal.y > 0)
        {
            //float hammerAngleGravityFactor = 1 - (.01f * (Vector2.up.y - (hammer.transform.position - goomba.transform.position).normalized.y));
            //gravityForce = new Vector2(0, (-Physics2D.gravity.y * goombaBody.gravityScale * goombaBody.mass) * hammerAngleGravityFactor);
            gravityForce = new Vector2(0, (-Physics2D.gravity.y * goombaBody.gravityScale * goombaBody.mass)) * .9f;
        }


        if (Vector2.Distance(pullForce, hammerToIntention.reactionForce) * Time.deltaTime < .1f)
        {
            float dampenerConstant = 100000;
            Debug.Log(-(goombaBody.velocity.normalized) * goombaBody.velocity.magnitude * Time.deltaTime * dampenerConstant);
            goombaBody.AddForce(-(goombaBody.velocity.normalized) * goombaBody.velocity.magnitude * Time.deltaTime * dampenerConstant);
        }

    }

    private void FixedUpdate()
    {
        pullForce = hammerToIntention.reactionForce;
    }

    // Update is called once per frame
    void Update()
    {
        intentionJoint.connectedAnchor = new Vector2(transform.position.x, transform.position.y);
        hammerToIntention.target = intention.transform.position;
        if (Vector2.Distance(pullForce, hammerToIntention.reactionForce) * Time.deltaTime < .1f)
        {
            intention.GetComponent<TargetJoint2D>().target = hammer.GetComponent<BoxCollider2D>().bounds.center;
        }


        if (GetComponent<Rigidbody2D>().velocity.x < -0.5)
        {
            goomba.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GetComponent<Rigidbody2D>().velocity.x > 0.5) 
        {
            goomba.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    public void hit(Vector2 hitDirection) 
    {
        Vector2 knockBack = new Vector2(100000, 100000);
        if (hitDirection.x < 0)
        {
            goombaBody.AddForce(knockBack * new Vector2(-1,1));
        }
        else 
        {
            goombaBody.AddForce(knockBack);
        }
    }
}
