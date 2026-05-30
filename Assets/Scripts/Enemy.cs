using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject floatingDamage;
    public EnemyConfig config;

    private HealthComponent healthComp;
    private float timeBtwAttack;
    private float stopTime;

    public Transform target;
    public Vector2 targetOffset;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private AddRoom room;
    [HideInInspector] public bool playerNotInRoom;
    private bool stopped;

    private Player player;          // оставим временно для движения, потом заменим на AI
    private HealthComponent playerHealth; // для нанесения урона

    private void Start()
    {
        healthComp = GetComponent<HealthComponent>();
        if (healthComp != null)
        {
            healthComp.OnDeath += Die;
            // Устанавливаем максимальное здоровье из конфига, если нужно
            // healthComp.Initialize(config.health); // предполагается, что метод есть
        }

        anim = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        room = GetComponentInParent<AddRoom>();

        // Поиск игрока (временный)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Player>();
            playerHealth = playerObj.GetComponent<HealthComponent>();
        }

        // Применяем параметры из конфига, если он назначен
        if (config != null)
        {
            timeBtwAttack = config.attackCooldown;
            // speed = config.speed; // если убрать свое поле speed
            // damage = config.damage; // если убрать свое поле damage
        }
    }

    private void FixedUpdate()
    {
        // Проверка остановки врага
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
            stopped = true;
        }

        // Движение к игроку (если не остановлен и игрок существует)
        if (!stopped && player != null)
        {
            Vector2 dir = (Vector2)player.transform.position - rb.position;
            rb.MovePosition(rb.position + dir.normalized * config.speed * Time.fixedDeltaTime);
        }

        // Поворот в сторону игрока
        if (player != null)
        {
            if (player.transform.position.x > transform.position.x)
                transform.eulerAngles = new Vector3(0, 180, 0);
            else
                transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    // Метод получения урона (теперь делегирует HealthComponent)
    public void TakeDamage(int damage)
    {
        stopTime = config.stopTimeOnHit; // берем из конфига
        healthComp.TakeDamage(damage);   // компонент сам обработает смерть

        // Визуализация урона
        Vector2 damagePos = new Vector2(transform.position.x + 1.75f, transform.position.y + 1.75f);
        Instantiate(floatingDamage, damagePos, Quaternion.identity);
        floatingDamage.GetComponentInChildren<FloatingDamage>().damage = damage;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerHealth != null)
        {
            if (timeBtwAttack <= 0)
            {
                anim.SetTrigger("attack");
                // Непосредственно атака вызывается из анимационного события OnEnemyAttack
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    // Вызывается из анимации
    public void OnEnemyAttack()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(config.damage);
            timeBtwAttack = config.attackCooldown;
        }
    }

    private void Die()
    {
        if (room != null && room.enemies != null)
            room.enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    // Flip пока не используется, можно удалить или оставить
}