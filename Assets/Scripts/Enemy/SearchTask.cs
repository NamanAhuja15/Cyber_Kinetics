using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SearchTask : Task
{
    private AI ai;
    private Transform player, lastposi;
    private NavMeshAgent navmeshagent;
    Animator anim;
    // Start is called before the first frame update
    public override bool Start()
    {
        navmeshagent.SetDestination(lastposi.position);
        anim.SetBool("walk", true);
        return true;
    }
    public SearchTask(TaskManager taskmanager, AI ai,Transform player,Transform lastposi,NavMeshAgent nav) : base(taskmanager)
    {
        this.ai = ai;
        this.navmeshagent = nav;
        this.player = player;
        this.lastposi = lastposi;
        this.anim = ai.gameObject.GetComponent<Animator>();
    }
    public override void Update()
    {
        if (lastposi != null)
        {
                if (navmeshagent.remainingDistance <= 2f)
                {
                    ai.GetComponent<Animator>().SetBool("walk", false);
                    navmeshagent.isStopped = true;
                }
            
            else
            {
                ai.GetComponent<Animator>().SetBool("walk", true);
                navmeshagent.isStopped = false;
                navmeshagent.SetDestination(lastposi.position);
            }
        }

    }
    // Update is called once per frame

}
