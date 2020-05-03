using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTask :Task
{
    private AI ai;
    void Start()
    {
        
    }
    public IdleTask(TaskManager taskmanager, AI ai) : base(taskmanager)
    {
        this.ai = ai;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
