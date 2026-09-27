using Unity.VisualScripting;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;
    PlayerMovement playerMovement;
    Animator animator;
    public float distance;
    Rigidbody2D rb;
    float moveSpeed = 2f;
    float chaseRange = 4f;
    public bool attackTriggered = false;

    [Header("Health & Knockback")]
    public float health = 30f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Soldier");
        }

        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(player.transform.position, transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        //Debug.Log(distance);

        bool isWalking = rb.linearVelocity.sqrMagnitude > 0.01f;
        animator.SetBool("IsWalking", isWalking);

        bool isClose = distance < 1.4f;
        animator.SetBool("IsClose", isClose);

        attackTriggered = isClose;

        if (isClose && !isKnockedBack)
        {
            playerMovement.TakeHit();
        }

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
        }

        if (direction[0] > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else if (direction[0] < 0)
        {    
            transform.localScale = new Vector3(-1,1,1);
        }  
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            return;
        }

        if (distance <= chaseRange && distance >= 1.4)
        { 
            Vector2 direction = (player.transform.position - transform.position).normalized;
            Debug.Log(direction[0]);
            rb.linearVelocity = direction * moveSpeed;

                  

        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection, float knockbackForce)
    {
        health -= damage;

        if (health <= 0f)
        {
            animator.SetTrigger("Died");
            Destroy(gameObject);
            return;
        }

        rb.linearVelocity = knockbackDirection * knockbackForce;
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
    }
}
