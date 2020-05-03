using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTask : Task
{
    private NavMeshAgent navmeshagent;
    private Transform player;
    private AI ai;
    public FollowTask(TaskManager taskManager,Transform player,AI ai,NavMeshAgent nav)
    : base(taskManager)
    {
        this.navmeshagent = nav;
        this.player = player;
        this.ai = ai;
    }
    // Start is called before the first frame update
    public override bool Start()
    {
        navmeshagent.SetDestination(player.position);
        ai.GetComponent<Animator>().SetBool("walk", true);
        return true;
    }

    // Update is called once per frame
   public override void Update()
    {
       // Debug.Log("Updating");
        if (navmeshagent.remainingDistance <= 2f)
        {
            ai.GetComponent<Animator>().SetBool("run", false);
            navmeshagent.isStopped = true;
        }
        else
        {
            ai.GetComponent<Animator>().SetBool("run", true);
            navmeshagent.isStopped = false;
            navmeshagent.SetDestination(player.position);
        }
    }
    public override bool Stop()
    {
        if (navmeshagent.remainingDistance <= 0.5f)
        {
            ai.gameObject.GetComponent<Animator>().SetBool("walk", false);
            isTaskCompleted = true;
           taskManager.OnTaskCompleted(this);
            return true;
        }
        else
            return false;
    }
}
