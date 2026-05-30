using UnityEngine;
using System;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int CurrentHealth { get; private set; }

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
            // Смерть — ответственность другого компонента
        }
    }

    public void Initialize(int maxHp)
    {
        maxHealth = maxHp;
        CurrentHealth = maxHp;
    }
}