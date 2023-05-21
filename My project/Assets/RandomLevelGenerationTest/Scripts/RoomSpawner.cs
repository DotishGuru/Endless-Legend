using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    /*
    1 --> need bottom door
    2 --> need top door
    3 --> need left door
    4 --> need right door
    */

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    void Start() {
        Destroy(gameObject, 4f);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>(); 
        Invoke("Spawn", 1f);   
    }

    void Spawn() {
        if(!spawned){
            switch (openingDirection)
            {
                case 1:
                    //need to spawn a room with a BOTTOM door
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                    break;
                case 2:
                    //need to spawn a room with a TOP room
                    rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                    break;
                case 3:
                    //need to spawn a room with a LEFT door
                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                    break;
                case 4:
                    //need to spawn a room with a RIGHT door
                    rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                    break;
            }    
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("SpawnPoint")){
            if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false){
                templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
