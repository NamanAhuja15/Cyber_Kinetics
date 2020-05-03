using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class
public class Task
{
    protected TaskManager taskManager;
    protected bool isTaskCompleted = false;

    public Task(TaskManager taskManager)
    {
        this.taskManager = taskManager;
    }

    public virtual bool Start()
    {
        return true;
    }

    public virtual bool Stop()
    {
        return true;
    }

    public virtual bool Pause()
    {
        return true;
    }

    public virtual bool Terminate()
    {
        return true;
    }

    public virtual bool IsRunning()
    {
        return true;
    }
    public virtual void Update()
    {
        
    }
}
