using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HearTask : Task
{
    // Start is called before the first frame update
    private AI ai;
    private Transform player,newposi;
    private GameObject enemy;
    private bool noisy;
    private float hearingdistance = 40f;
    public override bool Start()
    {
        return true;
    }
    public HearTask(TaskManager taskmanager, AI ai, Transform player) : base(taskmanager)
    {
        this.ai = ai;
        this.enemy = ai.gameObject;
        this.player = player;
    }
    // Update is called once per frame
   public override void Update()
    {
        noisy = ai.noisy;

    }

}
