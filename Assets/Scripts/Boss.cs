// Attached to boss enemy
// make sure bullet has a SLOW speed so player can feasibly dodge
// place enemy on FAR RIGHT of boundary and make him block path of character
// boss should not move

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Transform boundary;
    public Transform player;
    public GameObject shot;
    public Transform shotSpawn;
    public Transform batSpawn;
    public SpriteRenderer bat;
	public PlayerController2 pc;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Bat batScript;
    private Color origColor;
    private Color flashColor;
    private float redTime;    // next time for enemy to not be red anymore
    private float nextRed;    // how long for enemy to stay red when hit
    private float rightedge;  // position of right end of the boundary
    private float leftedge;   // position of left end of the boundary
    private float bottomedge; // position of bottom end of boundary
    private float topedge;    // position of top end of boundary
    private float nextFire;
    private float fireRate;
    private float nextSpawn;  // next time to spawn a new bat
    private float spawnRate;  // how long to wait before spawning another bat
    private int health;



    // Start is called before the first frame update
    void Start()
    {
        health = 80;
        redTime = 0;
        nextRed = 0.17f;
        nextFire = 0;
        fireRate = 2.17f;
        nextSpawn = 0;
        spawnRate = 6.17f;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        batScript = bat.GetComponent<Bat>();
        batScript.player = player;
        batScript.boundary = boundary;
        flashColor = Color.red;
        origColor = sprite.material.color;
        topedge = boundary.position.y + boundary.localScale.y / 2;
        leftedge = boundary.position.x - boundary.localScale.x / 2;
        rightedge = boundary.position.x + boundary.localScale.x / 2;
        bottomedge = boundary.position.y - boundary.localScale.y / 2;
    }

    void Update()
    {
        if (Time.time > redTime)  // restore color
        {
            sprite.material.color = origColor;
        }

        if (WithinBoundary() && Time.time > nextFire)  // shoot fire if player near and after fireRate time
        {
            Vector3 dir = player.position - transform.position;
            dir = dir.normalized;
            
            float ang = Vector2.Angle(dir, shotSpawn.up);
            
            Instantiate(shot, shotSpawn.position, Quaternion.AngleAxis(ang, shotSpawn.forward));

            nextFire = Time.time + fireRate;
        }

        if (WithinBoundary() && Time.time > nextSpawn)  // spawn bats if player near and after spawnRate time
        {
            Instantiate(bat, batSpawn.position, batSpawn.rotation);

            nextSpawn = Time.time + spawnRate;
        }
    }

    private bool WithinBoundary()
    {
        return player.position.x > leftedge && player.position.x < rightedge &&
            player.position.y > bottomedge && player.position.y < topedge;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (WithinBoundary() && other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            health = health - 1;
            sprite.material.color = flashColor;
            redTime = Time.time + nextRed;
        }
        if (health == 0)
        {	
			pc.winner ();
            // TODO: Create explosion?
            Destroy(this.gameObject);
        }
    }
}
