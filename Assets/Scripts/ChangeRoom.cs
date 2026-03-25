using UnityEngine;

public class ChangeRoom : MonoBehaviour
{

    public Vector3 cameraChangePos;
    public Vector3 playerChangePos;
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position += playerChangePos;
            cam.transform.position += cameraChangePos;
        }
    }
}
