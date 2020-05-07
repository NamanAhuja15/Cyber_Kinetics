using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public GameObject[] Players;
    public GameObject[] Enemies;
    public Transform[] spawnPoints;
    private float distance = 10000f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FindNearest(GameObject enemy)
    {
        for (int i = 0; i <= Players.Length - 1; i++)
        {
            float Player_distance = Vector3.Distance(enemy.transform.position, Players[i].transform.position);
            if (Player_distance <= distance)
            {
                distance = Player_distance;
            }
        }
    }
}
