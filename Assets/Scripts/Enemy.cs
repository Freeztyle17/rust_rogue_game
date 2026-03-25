using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject floatingDamage;

    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public int health = 3;

    public Transform target;
    public Vector2 targetOffset;

    public int damage;

    private float stopTime;
    public float startStopTime;

    public float speed;

    public Player player;

    private Animator anim;
    private Rigidbody2D rb;

    private bool facingRight = false;
    private SpriteRenderer sr;

    private AddRoom room;

    [HideInInspector] public bool playerNotInRoom;
    private bool stopped;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        room = GetComponentInParent<AddRoom>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!playerNotInRoom)
        {
            if (stopTime <= 0)
                stopped = false;
            else
            {
                stopped = true;
                stopTime -= Time.deltaTime;
            }
        }
        else
        {
            stopped= true;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
            //return;
        }


        if (!stopped)
        {
            Vector2 dir = (Vector2)player.transform.position - rb.position;
            rb.MovePosition(rb.position + dir.normalized * speed * Time.fixedDeltaTime);
        }


        if (player.transform.position.x > transform.position.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        health -= damage;

        Vector2 damagePos = new Vector2(transform.position.x + 1.75f, transform.position.y + 1.75f);
        Instantiate(floatingDamage, damagePos, Quaternion.identity);
        floatingDamage.GetComponentInChildren<FloatingDamage>().damage = damage;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (timeBtwAttack <= 0)
                anim.SetTrigger("attack");
            else
                timeBtwAttack -= Time.deltaTime;
        }
    }

    public void OnEnemyAttack()
    {
        player.ChangeHealth(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }

    private void Flip(float dirX)
    {
        if (dirX > 0 && !facingRight)
        {
            facingRight = true;
            sr.flipX = false;
        }
        else if (dirX < 0 && facingRight)
        {
            facingRight = false;
            sr.flipX = true;
        }
    }
}
