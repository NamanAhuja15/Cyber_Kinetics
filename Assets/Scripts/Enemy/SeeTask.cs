using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeeTask : Task
{
    private AI ai;
    private SphereCollider colli;
    private GameObject enemy,eye;
    private Transform player;
    public bool insight = false;
    public float fieldofviewangle;
    public Transform lastposition;
    // Start is called before the first frame update
   public override bool Start()
    {
        return true;
    }
    public override void Update()
    {
        ai.lastposition = lastposition;
        ai.insight = insight;
    }
    public SeeTask(TaskManager taskmanager, AI ai,Transform player,GameObject eye) : base(taskmanager)
    {
        this.ai = ai;
        this.enemy = ai.gameObject;
        this.player = player;
    }
 
  

}
