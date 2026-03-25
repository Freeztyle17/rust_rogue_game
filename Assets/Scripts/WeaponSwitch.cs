using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject gun;
    public GameObject sword;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gun.activeInHierarchy == true)
            {
                gun.SetActive(false);   
                sword.SetActive(true);
            }
            else if (sword.activeInHierarchy == true)
            {
                sword.SetActive(false);
                gun.SetActive(true);
            }
        }
    }
}
