using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
	//UI
	public AudioSource sound;
	public Text announce;
	public Text lives;
	public GameObject pauseMenu;
	public GameObject ui;
	public Image deadOverlay;
	public MenuStart menus;

	//SOUNDFX
	public AudioSource jumpSFX;
	public AudioSource shootSFX;
	public AudioSource dieSFX;

	//PHYSICS
    public float fallMultiplier = 15f;
    public float lowJumpMultiplier = 2f;
	public float maxSpeed;
    public float jumpForce = 1f;
	public float checkRadius;
	[Range(1, 10)]
	public float jumpVelocity;
	public Transform groundCheck;
	public LayerMask whatIsGround;

	//INTERNAL
	private Animator animator;
	private int life; 
	private bool alive;
	private bool jump;
	private bool isGrounded;
    private bool invincible;

	private float nextFire = 0.0f; // limit on how long to wait before firing again
	private float fireRate = 0.17f;   // adds small buffer on how much player can shoot per second
	private float moveForce = 10f;
	private float moveInput; // determine whether the left or the right keys are // pressed (to be edited bc touch movements)
    private float invTime = 0.0f; // time when invicibility runs out
    private float invLength = 1.7f; // length of invincibility

	private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private Vector2 velocity;
	private Vector2 prevVelocity;
    public static int gemCount;
	//GAMEPLAY
	public GameObject shot;  // references the bullet prefab
	public Transform shotSpawn;  // refrences the position the bullet will spawn
	public Joystick joys;
	public GameObject mobileUI;
    private int sceneNum;
	public Joystick jumpstick;

    void Start()
    {
		if (SystemInfo.deviceType == DeviceType.Desktop) {
			mobileUI.SetActive (false);
		}

		announce.text = "";
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        prevVelocity = Vector2.zero;
		alive = true;
        invincible = false;
		pauseMenu.SetActive (false);
		life = 3;
        jump = false;
        prevVelocity.x = 1;
        gemCount = 0;
		var tempColor = deadOverlay.color;
		tempColor.a= 0.0f;
		deadOverlay.color = tempColor;
		deadOverlay.enabled = false;
        sceneNum = SceneManager.GetActiveScene().buildIndex;
    }

	public void paused(bool temp) {
		if (temp == true) {
			//lives.text = "";
			ui.SetActive (!temp);
			//lives.fontSize = 0;
			sound.Pause();
			Time.timeScale = 0.0f;
		} else {
			ui.SetActive (!temp);
			sound.Play ();
			//lives.fontSize = 30;
			Time.timeScale = 1.0f;
		}
		pauseMenu.SetActive (temp);
	}

    // manage all the physics related stuff in the game
    void FixedUpdate()
    {
		lives.text = "Health: " + ((life * 30)) + " pts";
		if (life == 1) {
			lives.color = new Color(1f, 0.7f, 0.7f);
		}
        // left <- moveInput will be -1. 
        // right key pressed --> moveInput will be 1

		if (life == 0) {
			moveInput = 0.0f;
		}else if (SystemInfo.deviceType == DeviceType.Handheld) {
			moveInput = joys.Horizontal;
		} else if (SystemInfo.deviceType == DeviceType.Desktop) {
			moveInput = Input.GetAxis("Horizontal");
		} 

        rb.velocity = new Vector2(moveInput * maxSpeed, rb.velocity.y);

        // check if the character is going too fast
        // if it is going too fast (either left or right), then restrict the
        // velocity
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            float dirOfMovement = Mathf.Sign(rb.velocity.x);
            rb.velocity = new Vector2(dirOfMovement * maxSpeed, rb.velocity.y);
        }
        
        if (rb.velocity.x > 0 || rb.velocity.x < 0)
        {
            Flip(rb.velocity);
        } 
      
		if (alive && this.transform.position.y < -5.5) { //check if player has fallen below map
			dieSFX.Play ();
			alive = false;
		}

		if (!alive) {
			lives.text = "";
			sound.volume -= 0.05f;
			maxSpeed = 0.1f;
			animator.enabled = false;
			deadOverlay.enabled = true;
			mobileUI.SetActive (false);
			var tempColor = deadOverlay.color;
			tempColor.a += 0.01f;
			deadOverlay.color = tempColor;

			announce.text = "Game Over";
			menus.changeSceneWait ("Menu");
		}

		jumpHelper (false);
     
        animator.SetBool("grounded", isGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(moveInput));

    }

	public void jumpHelper(bool mobile) {
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
		if (alive && isGrounded && ((Input.GetKeyDown(KeyCode.Space) || (mobile && joys.Vertical > 0.8 && joys.Horizontal < 0.2))))
		{
			jump = true;
			jumpSFX.Play ();

		}
		if (alive && jump)
		{
			rb.AddForce(new Vector2(0.0f, jumpForce));
			jump = false;
		}
	}

    private void Update()
    {
        // form a downward vector from transform.position to groundCheck.position. The whole expression will return
        // true if the vector hits the layer "Ground".
		if (Input.GetButton("Fire1") && Time.time > nextFire && joys.Horizontal == 0 && joys.Vertical == 0 && isGrounded)  // mouse click, shots fired
        {
            Vector3 moveDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shotSpawn.position);
            moveDir.z = 0;
            moveDir.Normalize();
			shootSFX.Play ();

            float ang = Vector2.Angle(moveDir, shotSpawn.up);
            
            if (moveDir.x > shotSpawn.up.x)  // right side shooting
            {
                if (prevVelocity.x < 0)   // shoots behind
                {
                    sprite.flipX = !sprite.flipX;
                    prevVelocity.x = 1;
                }
                Instantiate(shot, shotSpawn.position, Quaternion.AngleAxis(-ang, shotSpawn.forward));

            }
            else   // left side
            {
                if (prevVelocity.x > 0)   // shoots behind
                {
                    sprite.flipX = !sprite.flipX;
                    prevVelocity.x = -1;
                }
                Instantiate(shot, shotSpawn.position, Quaternion.AngleAxis(ang, shotSpawn.forward));
            }

            nextFire = Time.time + fireRate;  // updates limit for next shot
        }

        if (Time.time > invTime)  // reset player to vulnerable
        {
            invincible = false;
            // restore opacity
            float b = sprite.material.color.b;
            float g = sprite.material.color.g;
            float r = sprite.material.color.r;
            sprite.material.SetColor("_Color", new Color(b, g, r, 1.0f));
        }

        //Debug.Log("gemCount " + gemCount + " sceneNum " + sceneNum);
        if (gemCount == 12)
        {
            TransitionScene();
        }
    }

    void TransitionScene()
    {
        if (sceneNum == 0 && rb.position.x > 11.4)
        {
            SceneManager.LoadScene(1);
            //gemCount = 0;

        }
        else if (sceneNum == 1 && rb.position.x > 12.4)
        {
            SceneManager.LoadScene(2);
           /// gemCount = 0;
        }
        else if (sceneNum == 2 && rb.position.x > 11.4)
        {
            SceneManager.LoadScene(3);
            //gemCount = 0;
        }
    }

    void Flip(Vector2 currVelocity)
    {
        if ((currVelocity.x > 0.0f && prevVelocity.x < 0.0f) 
            || currVelocity.x < 0.0f && prevVelocity.x > 0.0f)
        {
            sprite.flipX = !sprite.flipX;

        }
       prevVelocity = currVelocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Enemy" && !invincible)
        {
            invincible = true;
            life = life - 1;
            invTime = Time.time + invLength;

            // lower opacity
            float b = sprite.material.color.b;
            float g = sprite.material.color.g;
            float r = sprite.material.color.r;
            sprite.material.SetColor("_Color", new Color(b, g, r, 0.5f));

            // bounce back
            float force = 1017.0f;
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            other.GetContacts(contacts);
            Vector2 dir = contacts[0].point - (new Vector2(transform.position.x, transform.position.y));
            dir = -dir.normalized;
            //dir.y = 0.05f;   // small upward force
            rb.AddForce(dir * force);
            
        }

        if (life == 0)
        {
			dieSFX.Play ();
            alive = false;
        }
    }
}
