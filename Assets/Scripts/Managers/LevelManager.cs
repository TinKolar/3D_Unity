using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    GameObject door;
    Door d;
    bool doorOpened = false;

    private void Start()
    {
        door = GameObject.FindGameObjectWithTag("Door");
        d = door.GetComponent<Door>();
    }

    private void Update()
    {
        if (!doorOpened && AreAllEnemiesDead())
        {
            d.OpenDoor();
            doorOpened = true;
        }
    }

    private bool AreAllEnemiesDead()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("KeyEnemy");
        return enemies.Length == 0;
    }
}
