using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    GameManager gamemanager;

    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "Player") 
    //    {
    //        gamemanager.levelSpawner();
    //    }
    //}
}
