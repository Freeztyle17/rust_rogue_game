using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public int damage;

    public LayerMask whatIsSolid;

    private Vector2 lastPos;

    void Start()
    {
        lastPos = transform.position;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        Vector2 currentPos = transform.position;
        Vector2 direction = (currentPos - lastPos).normalized;
        float distance = Vector2.Distance(currentPos, lastPos);

        // Raycast между прошлой и новой позицией (главное исправление)
        RaycastHit2D hit = Physics2D.Raycast(lastPos, direction, distance, whatIsSolid);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>()?.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }

        // теперь двигаем пулю
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);

        lastPos = currentPos;
    }
}
