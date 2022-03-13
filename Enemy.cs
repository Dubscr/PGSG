using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;

    [Header("Spotting Range")]
    [Range(0, 10)]
    [SerializeField] private float minSpottingRange;
    [Range(0, 100)]
    [SerializeField] private float maxSpottingRange;

    [Header("Jump")]
    [Tooltip("Range required from left/right of enemy to jump")]
    [SerializeField] private float jumpRange;
    [Tooltip("Force of enemy jumping")]
    [SerializeField] private float jumpForce;
    [Tooltip("When enemy jumps, stop increasing velocity here")]
    [SerializeField] private float maxJumpVelocity;
    [Tooltip("Move speed")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private EdgeCollider2D top;

    // Start is called before the first frame update
    void Start()
    {
        top = GetComponent<EdgeCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir;
        if(Mathf.Abs(target.position.x - transform.position.x) > minSpottingRange)
        {
            if (target.position.x - transform.position.x > 0)
            {
                dir = Vector2.right;
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            else
            {
                dir = Vector2.left;
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
            RaycastHit2D left = Physics2D.Raycast(transform.position, dir, jumpRange);
            if (left.collider != null)
            {
                if (left.collider.CompareTag("Ground") && rb.velocity.y < maxJumpVelocity)
                {
                    Jump();
                }
            }
        } else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
