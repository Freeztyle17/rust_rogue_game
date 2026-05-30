using UnityEngine;
using System;

public class ShieldComponent : MonoBehaviour
{
    [SerializeField] private float maxTime = 10f;
    private float currentTime;
    private bool isActive;

    public event Action<bool> OnShieldStateChanged;
    public event Action<float> OnTimeChanged;

    public void Activate()
    {
        isActive = true;
        currentTime = maxTime;
        OnShieldStateChanged?.Invoke(true);
        OnTimeChanged?.Invoke(currentTime);
    }

    public void ReduceTime(float amount)
    {
        if (!isActive) return;
        currentTime -= amount;
        OnTimeChanged?.Invoke(currentTime);
        if (currentTime <= 0)
            Deactivate();
    }

    public void Deactivate()
    {
        isActive = false;
        OnShieldStateChanged?.Invoke(false);
    }

    public bool IsActive() => isActive;
}