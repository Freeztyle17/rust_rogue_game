using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    [HideInInspector] public float damage;
    private TextMesh textMesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = "-" + damage;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnimationOver()
    {
        Destroy(gameObject);
    }
}
