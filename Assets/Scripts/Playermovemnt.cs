using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Playermovemnt : MonoBehaviour
{
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField]
    private float dirX = 0f;
    [SerializeField]
    private float movespeed = 7f;
    [SerializeField]
    private float jumpforce = 8f;

    [SerializeField] private LayerMask jumpableGround;

    private Vector3 respawnPoint;
    public GameObject fallDetector;

    private enum MovementState { idle, running, jump, fall }

    [SerializeField] private AudioSource jumpSoundeffect;
    bool canDoubleJump;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2 (dirX * movespeed,rb.velocity.y);
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
           
            rb.velocity = new Vector2 (rb.velocity.x,jumpforce);
            jumpSoundeffect.Play();
            canDoubleJump =true;
        }
        else if (canDoubleJump && Input.GetButtonDown("Jump"))
        {
            
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            jumpSoundeffect.Play();
            canDoubleJump =false;
        }
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);

        
        UpdateAnimationState();

    }
    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.fall;
        }

        anim.SetInteger("state", (int)state);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "FallDetector")
        {
            
            transform.position = respawnPoint;
        }
    }
}
