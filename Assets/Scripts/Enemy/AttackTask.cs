using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : Task
{
    private AI ai;
    private Transform player;
    // Start is called before the first frame update
    public AttackTask(TaskManager taskmanager, AI ai,Transform player) : base(taskmanager)
    {
        this.ai = ai;
        this.player = player;
    }
   public override bool  Start()
    {
        ai.gameObject.GetComponent<Animator>().SetBool("Attack", true);
        return true;
    }

    // Update is called once per frame
  public override  void Update()
    {
       if(Vector3.Distance(ai.transform.position,player.position)>=2f)
        {
            ai.gameObject.GetComponent<Animator>().SetBool("Attack", false);
        }
    }
}
