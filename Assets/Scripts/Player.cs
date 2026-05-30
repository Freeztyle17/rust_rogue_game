using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


public class Player : MonoBehaviour
{
    [Header("Configuration")]
    public PlayerConfig playerConfig;

    [Header("Control")]
    public ControlType controlType;
    public Joystick joystick;
    public enum ControlType { PC, Android };

    // Ссылки на другие компоненты
    private PlayerMovement movement;
    private HealthComponent healthComp;
    private WeaponInventory weaponInventory;
    private ShieldComponent shieldComp;
    private Animator anim;

    private Vector2 moveInput;
    private bool facingRight = true;

    void Start()
    {

        movement = GetComponent<PlayerMovement>();
        healthComp = GetComponent<HealthComponent>();
        weaponInventory = GetComponent<WeaponInventory>();
        shieldComp = GetComponent<ShieldComponent>();
        anim = GetComponent<Animator>(); 

        if (playerConfig != null && healthComp != null)
            healthComp.Initialize(playerConfig.maxHealth);

        if (healthComp != null)
        {
            healthComp.OnHealthChanged += (hp) =>
            {
                // Если есть ссылка на healthDisplay, обновите здесь или через UI-компонент
                // healthDisplay.text = "HP: " + hp;
            };
            // Смерть теперь обрабатывает GameManager, так что здесь ничего не делаем
        }

        if (controlType == ControlType.PC && joystick != null)
            joystick.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Обработка ввода в зависимости от платформы
        if (controlType == ControlType.PC)
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        else if (controlType == ControlType.Android && joystick != null)
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);

        // Передаём ввод движению
        if (movement != null)
            movement.MoveInput = moveInput;

        // Анимация бега
        if (anim != null)
            anim.SetBool("isRunning", moveInput.sqrMagnitude > 0);

        // Поворот персонажа
        if (!facingRight && moveInput.x > 0)
            Flip();
        else if (facingRight && moveInput.x < 0)
            Flip();

        if (Input.GetKeyDown(KeyCode.Q) && weaponInventory != null)
            weaponInventory.SwitchToNext();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            interactable.Interact(gameObject);
    }

    
}