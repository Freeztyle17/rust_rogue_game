using UnityEngine;

public class Gun : MonoBehaviour
{
    public float offset;
    public float mouseX; 
    public GameObject bullet;
    public Transform shotPoint;
    public Joystick joystick;

    private float timeBtwShots;
    public float startTimeBtwShots;

    //05.01.26 UPD
    private Player player;
    private Vector3 difference;
    private float rotZ;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(player.controlType == Player.ControlType.PC)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {


        if(player.controlType == Player.ControlType.PC)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            mouseX = mousePos.x;

            difference = mousePos - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        else if(player.controlType == Player.ControlType.Android && Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f)
        {
            rotZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;
        }
        

        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0) && player.controlType == Player.ControlType.PC)
            {
                Shoot();
            }
            else if(player.controlType == Player.ControlType.Android)
            {
                if(joystick.Horizontal != 0 || joystick.Vertical != 0)
                {
                    Shoot();
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

    }

    public void Shoot()
    {
        Instantiate(bullet,  shotPoint.position, transform.rotation);
        timeBtwShots = startTimeBtwShots;
    }
}
