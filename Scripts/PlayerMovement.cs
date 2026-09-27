using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float moveSpeed = 5f;
    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
  
    public float health = 100;
    public float hitCooldown = 0.6f;
    public float hitTimer = 0f;

    [Header("Attack")]
    public float attackDamage = 20f;
    public float attackRange = 2f;
    public float attackCooldown = 0.4f;
    public float knockbackForce = 6f;
    public LayerMask enemyLayer;
    private float attackTimer = 0f;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        bool isWalking = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("IsWalking", isWalking);

        hitTimer -= Time.deltaTime;

        /*if (mob.attackTriggered && hitTimer <= 0f)
        {
            //mob.attackTriggered = false;

                animator.SetTrigger("Attacked");
                health -= 1;
                hitTimer = hitCooldown;

        }*/

        //wasAttacking = mob.attackTriggered;

        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        if (Input.GetKeyDown(KeyCode.X) /*&& attackTimer <= 0f*/)
        {
            Attack();
            attackTimer = attackCooldown;
        }

        Debug.Log(health);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void TakeHit()
    {
        if (hitTimer <= 0f)
        {
                
                animator.SetTrigger("Attacked");
                health -= 1;
                hitTimer = hitCooldown;
        }
    }

    void Attack()
    {
        animator.ResetTrigger("sword");
        animator.SetTrigger("sword");
        //animator.ResetTrigger("sword");

        Vector2 attackDir = facingRight ? Vector2.right : Vector2.left;
        Vector2 attackPos = (Vector2)transform.position + attackDir * 0.5f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, attackDamage, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            MobMovement mob = hit.GetComponent<MobMovement>();
            if (mob != null)
            {
                mob.TakeDamage(attackDamage, attackDir, knockbackForce);
            }
        }
    }
}
