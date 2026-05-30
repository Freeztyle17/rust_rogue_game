using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float cooldown;
    [HideInInspector] public bool isCooldown;

    private Image shieldImage;
    private ShieldComponent shieldComponent;  // Теперь ссылаемся на компонент щита
    private GameObject shieldVisual;          // Визуальный объект щита (настраивается в инспекторе)

    void Start()
    {
        shieldImage = GetComponent<Image>();

        // Ищем ShieldComponent на игроке
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            shieldComponent = player.GetComponent<ShieldComponent>();
            
            // Подписываемся на события щита
            if (shieldComponent != null)
            {
                shieldComponent.OnShieldStateChanged += OnShieldStateChanged;
                shieldComponent.OnTimeChanged += OnTimeChanged;
            }
        }

        isCooldown = true;
    }

    void Update()
    {
        if (isCooldown)
        {
            shieldImage.fillAmount -= 1 / cooldown * Time.deltaTime;

            if (shieldImage.fillAmount <= 0)
            {
                shieldImage.fillAmount = 1;
                isCooldown = false;
                
                // Вместо прямого обращения к player.shield используем событие
                if (shieldComponent != null)
                {
                    shieldComponent.Deactivate();  // Нужно добавить публичный метод в ShieldComponent
                }
                
                gameObject.SetActive(false);
            }
        }
    }

    // Вызывается при изменении состояния щита
    private void OnShieldStateChanged(bool active)
    {
        if (!active)
        {
            // Щит деактивирован - сбрасываем UI
            shieldImage.fillAmount = 1;
            isCooldown = true;
            gameObject.SetActive(false);
        }
    }

    // Вызывается при изменении времени щита
    private void OnTimeChanged(float time)
    {
        // Можно обновить UI в зависимости от оставшегося времени
        // float normalizedTime = time / maxTime;
        // shieldImage.fillAmount = normalizedTime;
    }

    public void ResetTimer()
    {
        shieldImage.fillAmount = 1;
    }

    // Этот метод больше не должен принимать damage как int
    public void ReduceTime(int damage)
    {
        shieldImage.fillAmount += damage / 5f;
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        if (shieldComponent != null)
        {
            shieldComponent.OnShieldStateChanged -= OnShieldStateChanged;
            shieldComponent.OnTimeChanged -= OnTimeChanged;
        }
    }
}