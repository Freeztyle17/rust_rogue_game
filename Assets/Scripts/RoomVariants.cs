using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class RoomVariants : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] rightRooms;
    public GameObject[] leftRooms;
    
    public GameObject key;
    public GameObject gun;
    public GameObject sword;

    [HideInInspector] public List<GameObject> rooms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RandomSpawner());
    }

    IEnumerator RandomSpawner()
    {
        yield return new WaitForSeconds(6f);
        AddRoom lastRoom = rooms[rooms.Count - 1].GetComponent<AddRoom>();
        int rand = Random.Range(0, rooms.Count - 2);
        int randGun = Random.Range(0, rooms.Count - 3);
        int randSword = Random.Range(0, rooms.Count - 3);

        Instantiate(key, rooms[rand].transform.position, Quaternion.identity);
        Instantiate(gun, rooms[randGun].transform.position, Quaternion.identity);
        Instantiate(sword, rooms[randSword].transform.position, Quaternion.identity);

        lastRoom.door.SetActive(true);
        lastRoom.DestroyWalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
