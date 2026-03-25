using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    //05.01.26 UPD
    public ControlType controlType;
    public Joystick joystick;
    public enum ControlType{PC, Android};

    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Animator anim;

    private bool facingRight = true;

    [Header("Key")]
    public GameObject keyIcon;
    private bool keyButtonPushed;

    [Header("Health")]
    public float health;
    public Text healthDisplay;

    [Header("Shield")]
    public GameObject shield;
    public Shield shieldTimer;

    [Header("Weapons")]
    public List<GameObject> unlockedWeapons;
    public GameObject[] allWeapons;
    public Image weaponIcon;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //05.01.26 UPD
        if(controlType == ControlType.PC)
        {
            joystick.gameObject.SetActive(false);
        }

        anim = GetComponent<Animator>();

        anim.SetBool("isRunning", moveInput.sqrMagnitude > 0);

        if (unlockedWeapons.Count > 0)
        {
            unlockedWeapons[0].SetActive(true);
            weaponIcon.sprite = unlockedWeapons[0].GetComponent<SpriteRenderer>().sprite;
            weaponIcon.SetNativeSize();
        }
    }

    void Update()
    {
        //05.01.26 UPD
        if(controlType == ControlType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); 
        }
        else if(controlType == ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        
        moveVelocity = moveInput.normalized * speed;



        if (!facingRight && moveInput.x > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput.x < 0)
        {
            Flip();
        }

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    } 

    void FixedUpdate()
    {
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        facingRight = !facingRight;

        float offset = 1.6f; // ��������, ����� �������� �� �������� ��� �����
        Vector3 pos = transform.position;
        pos.x += facingRight ? offset : -offset;
        transform.position = pos;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Potion"))
        {
            ChangeHealth(5); // todo ������ �� �� ������ ������������
            Destroy(other.gameObject);
        } else if (other.CompareTag("Shield"))
        {
            if(!shield.activeInHierarchy)
            {
                shield.SetActive(true);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                Destroy(other.gameObject);
            }
            else
            {
                shieldTimer.ResetTimer();
                Destroy(other.gameObject);
            }

        }
        else if (other.CompareTag("Weapon"))
        {
            for (int i = 0; i < allWeapons.Length; i++)
            {
                if (other.name == allWeapons[i].name)
                {
                    unlockedWeapons.Add(allWeapons[i]);
                }
            }
            SwitchWeapon();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Key"))
        {
            keyIcon.SetActive(true);
            Destroy(other.gameObject); 
        }
    }

    public void ChangeHealth(int healthValue)
    {
        if (!shield.activeInHierarchy || shield.activeInHierarchy && healthValue > 0)
        {
            health += healthValue;
            healthDisplay.text = "HP: " + health;
        } 
        else if (shield.activeInHierarchy && healthValue < 0) 
        {
            shieldTimer.ReduceTime(healthValue);
        }
    }

    public void SwitchWeapon()
    {
        if (unlockedWeapons.Count == 0) return;

        int activeIndex = -1;

        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (unlockedWeapons[i].activeSelf)
            {
                activeIndex = i;
                break;
            }
        }

        // ���� ������ �� ������� � ���������� ������
        if (activeIndex == -1)
        {
            unlockedWeapons[0].SetActive(true);
            weaponIcon.sprite = unlockedWeapons[0].GetComponent<SpriteRenderer>().sprite;
            return;
        }

        // ��������� �������
        unlockedWeapons[activeIndex].SetActive(false);

        // �������� ��������� �� �����
        int next = (activeIndex + 1) % unlockedWeapons.Count;

        unlockedWeapons[next].SetActive(true);
        weaponIcon.sprite = unlockedWeapons[next].GetComponent<SpriteRenderer>().sprite;
        weaponIcon.SetNativeSize();
    }

    public void OnKeyButtonDown()
    {
        keyButtonPushed = !keyButtonPushed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Door") && keyButtonPushed && keyIcon.activeInHierarchy)
        {
            keyIcon.SetActive(false);
            other.gameObject.SetActive(false);
            keyButtonPushed= false;
        }
    }
}
