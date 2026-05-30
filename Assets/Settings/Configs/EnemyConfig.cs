using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "Configs/Enemy")]
public class EnemyConfig : ScriptableObject
{
    public int health = 3;
    public int damage = 1;
    public float speed = 2f;
    public float attackCooldown = 1f;
    public float stopTimeOnHit = 0.5f; // задержка при получении урона
}