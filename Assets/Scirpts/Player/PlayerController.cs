using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed;
    public float jumpForce;

    [Header("Player State")]
    public float health;
    public bool isDead;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround;
    public bool isJump;
    public bool canJump;

    [Header("Jump FX")]
    public GameObject landFX;
    public GameObject jumpFX;

    [Header("Attack Settings")]
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackRate;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;

        CheckInput();
    }

    public void FixedUpdate()
    {
        if(isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        PhysicsCheck();
        Movement();
        Jump();

    }

    void CheckInput()
    {
        if(Input.GetButtonDown("Jump") && isGround)
        {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if(horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput,1,1);
        }
    }
    void Jump()
    {
        if (canJump)
        {
            isJump = true;
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = 4;
            canJump = false;

        }
    }

    public void Attack()
    {
        if(Time.time > nextAttack)
        {
            Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);

            nextAttack = Time.time + attackRate;
        }
    }

    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position,checkRadius,groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            isJump = false;
        }
        else
        {
            rb.gravityScale = 4;
        }
    }

    public void LandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))
        {
            health -= damage;
            if (health < 1)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");
        }
    }
}
