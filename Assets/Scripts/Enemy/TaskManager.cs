using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TaskManager : MonoBehaviour
{
    private Queue<Task> pendingTasks = new Queue<Task>();
    private Task currentTask = null;
    public static TaskManager instance;
    public delegate void TaskComplete(Task task);
    public static TaskComplete OnTaskComplete;
    private void Start()
    {
        instance = this;
        StartTask(new Task(this));
    }

    private void Update()
    {
        if (currentTask != null)
        {
            if (currentTask.Start())
            {
                   currentTask.Update();
            }
        }
        for(int i=0;i<pendingTasks.Count;i++)
        {
            StartTask(pendingTasks.Dequeue());
        }
    }
    public void AddTask(Task task)
    {
        pendingTasks.Enqueue(task);
    }
    public void StartTask(Task task)
    {
        if (task == null)
            return;

        if(currentTask != null && currentTask != task )
        {
            if(currentTask.Stop())
            {
                currentTask = task;
                currentTask.Start();
            }
            else
            {
                pendingTasks.Enqueue(task);
            }
        }
        else
        {
            currentTask = task;
            currentTask.Start();
        }
    }
    public void CreateFollow(AI ai,Transform player,NavMeshAgent nav)
    {
            AddTask(new FollowTask(this, player, ai,nav));
      
    }
    public void CreateWake(AI ai)
    {
            AddTask(new WakeupTask(this, ai));
        
    }
    public void CreateIdle(AI ai)
    {
        AddTask(new IdleTask(this, ai));
    }
public void CreateAttack(AI ai,Transform player)
    {
        AddTask(new AttackTask(this, ai,player));
    }
    public void CreateSearch(AI ai, Transform player, Transform lastposi, NavMeshAgent nav)
   {
       AddTask(new SearchTask(this, ai,player,lastposi,nav));
    }

    public void OnTaskCompleted(Task task)
    {
        if(task != null && task == currentTask)
        {
            var nextTask = pendingTasks.Dequeue();
            if (nextTask != null)
            {
                currentTask = nextTask;
                currentTask.Start();
            }
            else
                currentTask = null;
        }
    }
}
