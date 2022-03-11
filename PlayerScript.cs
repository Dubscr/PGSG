using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Transform groundCheck;
    private bool isGrounded;
    private bool m_FacingRight = true;
    private float xInput;
    private float speed;
    private AudioSource AS;
    private bool isJumping;
    private bool isWalking;

    [Header("Customization")]
    [SerializeField] private float airSpeed;
    [SerializeField] private float groundSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float walkInterval;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip landClip;
    // Start is called before the first frame update
    void Start()
    {
        groundCheck = GetComponentInChildren<Transform>().GetChild(0);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        AS = GetComponent<AudioSource>();

        StartCoroutine(WalkNoise());
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, LayerMask.GetMask("Ground"));
    }
    // Update is called once per frame
    void Update()
    {
        Input_();
        Movement();
        Animation();
        Audio();

        if (xInput < 0 && m_FacingRight)
        {
            Flip();
        }
        if (xInput > 0 && !m_FacingRight)
        {
            Flip();
        }

        if(rb.velocity.y < -0.05f)
        {
            isJumping = true;
        }

        if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
        {
            if (!isWalking && !isJumping)
            {
                StartCoroutine(WalkNoise());
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Input_()
    {
        xInput = Input.GetAxisRaw("Horizontal");
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            AS.clip = jumpClip;
            AS.Play();
        }

        if (isGrounded)
        {
            speed = groundSpeed;
        } 
        else
        {
            speed = airSpeed;
        }
    }
    private void Animation()
    {
        anim.SetFloat("Move", Mathf.Abs(xInput));
        anim.SetFloat("Velocity", rb.velocity.y);
    }

    private void Audio()
    {
        if (isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
            }
        }

    }

    IEnumerator WalkNoise()
    {
        isWalking = true;

        if (isGrounded && !isJumping)
        {
            if (AS.clip != walkClip)
            {
                yield return new WaitForSeconds(0.1f);
                AS.clip = walkClip;
            }

            AS.Play();
        }

         yield return new WaitForSeconds(walkInterval);

        isWalking = false;

    }
}
