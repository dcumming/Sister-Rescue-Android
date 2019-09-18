// Attach to bat enemy
// make sure rigidbody physics have gravity set to 0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Transform boundary;
    public Transform player;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Color origColor;
    private Color flashColor;
    private float redTime;    // next time for enemy to not be red anymore
    private float nextRed;    // how long for enemy to stay red when hit
    private float rightmove;
    private float leftmove;
    private float speed;
    private float rightedge;  // position of right end of the boundary
    private float leftedge;   // position of left end of the boundary
    private float bottomedge; // position of bottom end of boundary
    private float topedge;    // position of top end of boundary
    private bool direction;
    private bool inside;
    private int health;


    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        speed = 0.5f;
        redTime = 0;
        nextRed = 0.17f;
        inside = false;
        direction = false;
        rightmove = 1.0f;
        leftmove = -1.0f;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        flashColor = Color.red;
        origColor = sprite.material.color;
        topedge = boundary.position.y + boundary.localScale.y / 2;
        leftedge = boundary.position.x - boundary.localScale.x / 2;
        rightedge = boundary.position.x + boundary.localScale.x / 2;
        bottomedge = boundary.position.y - boundary.localScale.y / 2;
    }

    void Update()
    {
        inside = WithinBoundary();

        if (Time.time > redTime)  // restore color
        {
            sprite.material.color = origColor;
        }
    }

    void FixedUpdate()
    {
        // change direction if facing right and past boundary on right
        // change direction if facing left and past boundary on left
        if ((transform.position.x > rightedge && direction) ||
            (transform.position.x < leftedge && !direction))
        {
            //sprite.flipX = !sprite.flipX;
            direction = !direction;
        }

        if ((direction && !sprite.flipX) || (!direction && sprite.flipX))
        {
            sprite.flipX = !sprite.flipX;
        }

        if (inside)  // player is in position to be attacked
        {
            Vector3 dir = player.position - transform.position;
            dir = dir.normalized;
            rb.velocity = new Vector2(dir.x * speed * 3, dir.y * speed * 3);
        }
        else
        {
            // move bat back to top of boundary
            if (transform.position.y < topedge)
            {
                Vector3 up = (new Vector3(0, topedge, 0)) - transform.position;
                up = up.normalized;
                rb.velocity = new Vector2(0, up.y * speed);
            }

            // move bat in horizontal motion
            if (direction)   // facing right, so move right
            {
                rb.velocity = new Vector2(rightmove * speed, rb.velocity.y);
            }
            else   // facing left, so move left
            {
                rb.velocity = new Vector2(leftmove * speed, rb.velocity.y);
            }
        }
    }

    private bool WithinBoundary()
    {
        return player.position.x > leftedge && player.position.x < rightedge &&
            player.position.y > bottomedge && player.position.y < topedge;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            health = health - 1;
            sprite.material.color = flashColor;
            redTime = Time.time + nextRed;
        }
        if (health == 0)
        {
            // TODO: Create explosion?
            Destroy(this.gameObject);
        }
    }
}
