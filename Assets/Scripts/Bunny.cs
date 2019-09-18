// Attached to bunny enemy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public Transform boundary;

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
    private bool direction;
    private int health;


    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        speed = 0.25f;
        redTime = 0;
        nextRed = 0.17f;
        direction = false;
        rightmove = 1.0f;
        leftmove = -1.0f;    
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        flashColor = Color.red;
        origColor = sprite.material.color;
        leftedge = boundary.position.x - boundary.localScale.x / 2;
        rightedge = boundary.position.x + boundary.localScale.x / 2;
    }

    void Update()
    {
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
            sprite.flipX = !sprite.flipX;
            direction = !direction;
        }

        // move bunny
        if (direction)   // facing right, so move right
        {
            rb.velocity = new Vector2(rightmove * speed, rb.velocity.y);
        }
        else   // facing left, so move left
        {
            rb.velocity = new Vector2(leftmove * speed, rb.velocity.y);
        }

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