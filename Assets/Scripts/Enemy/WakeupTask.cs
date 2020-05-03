using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeupTask : Task
{
    // Start is called before the first frame update
    private AI ai;

    public override bool  Start()
    {
        ai.StartCoroutine(Wakeup());
        return true;
    }

    // Update is called once per frame
    public WakeupTask(TaskManager taskmanager,AI ai):base(taskmanager)
    {
        this.ai = ai;
    }
    IEnumerator Wakeup()
    {
        ai.GetComponent<Animator>().SetTrigger("standup");
        yield return new WaitForSeconds(2f);
        ai.OnAwake();
        isTaskCompleted = true;
        taskManager.OnTaskCompleted(this);
    }
    public override bool Stop()
    {
        return isTaskCompleted;
    }


}
