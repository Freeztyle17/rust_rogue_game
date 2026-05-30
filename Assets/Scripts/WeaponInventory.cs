using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> unlockedWeapons;
    [SerializeField] private Image weaponIcon; // можно потом перенести в UI-контроллер

    public GameObject CurrentWeapon { get; private set; }
    public event System.Action<GameObject> OnWeaponChanged;

    private void Start()
    {
        if (unlockedWeapons.Count > 0)
            SwitchTo(0);
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        if (!unlockedWeapons.Contains(weaponPrefab))
        {
            unlockedWeapons.Add(weaponPrefab);
            SwitchTo(unlockedWeapons.Count - 1);
        }
    }

    public void SwitchToNext()
    {
        if (unlockedWeapons.Count == 0) return;

        int currentIndex = unlockedWeapons.IndexOf(CurrentWeapon);
        int nextIndex = (currentIndex + 1) % unlockedWeapons.Count;
        SwitchTo(nextIndex);
    }

    public void SwitchTo(int index)
    {
        if (unlockedWeapons.Count == 0 || index < 0 || index >= unlockedWeapons.Count) return;

        if (CurrentWeapon != null)
            CurrentWeapon.SetActive(false);

        CurrentWeapon = unlockedWeapons[index];
        CurrentWeapon.SetActive(true);

        if (weaponIcon != null)
        {
            SpriteRenderer sr = CurrentWeapon.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                weaponIcon.sprite = sr.sprite;
                weaponIcon.SetNativeSize();
            }
        }

        OnWeaponChanged?.Invoke(CurrentWeapon);
    }
}