using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public bool Awake = false;
    public NavMeshAgent nav;
    public Transform lastposition;
    private Transform player;
    private AIManager manager;
    public float range,fieldofviewangle,hearingdistance;
    public bool detected = false;
    private TaskManager taskmanager;
    public GameObject eye;
    public bool insight = false;
    public bool noisy = false;
    private SphereCollider colli;
    private float time;
    public enum State
    {
        NONE,IDLE,PATROL,ALERT,ATTACK       
    }
    // Start is called before the first frame update
    void Start()
    {
        nav = this.gameObject.GetComponent<NavMeshAgent>();
        taskmanager = this.gameObject.GetComponent<TaskManager>();
        colli = this.gameObject.GetComponent<SphereCollider>();
        lastposition = null;
        manager = GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerinRange();
        Hear();
        
        if (insight)
        {
            taskmanager.CreateFollow(this, player, nav);
            detected = true;
            time = 0f;
        }
        else
        {
            if (detected)
            {
                taskmanager.CreateSearch(this, player, lastposition, nav);
                if (time > 3f)
                    detected = false;
            }
        }
        time += Time.deltaTime;
    }
    public bool isAwake()
    {
        return Awake;
    }
    public void OnAwake()
    {
        Awake = true;
        gameObject.GetComponent<Animator>().SetBool("idle", true);
    }
    public virtual void SwitchState(State state)
    {

    }
    private void PlayerinRange()
    {
        if (Vector3.Distance(transform.position, player.position) <= range)
        {
            if (!Awake)
            {
                taskmanager.CreateWake(this);
            }
        }
    }
    private void Hear()
    {
        if (noisy)
        {
            if (CalculateLength(player) <= hearingdistance)
            {
                lastposition.position = player.position;
            }
        }
    }
    private float CalculateLength(Transform player)
    {
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(player.position, path);
        Vector3[] waypoints = new Vector3[path.corners.Length + 2];
        waypoints[0] = transform.position;
        waypoints[waypoints.Length - 1] = player.position;

        for (int i = 0; i < path.corners.Length; i++)
        {
            waypoints[i + 1] = path.corners[i];
        }
        float pathlength = 0f;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            pathlength += Vector3.Distance(waypoints[i], waypoints[i + 1]);
        }
        return pathlength;
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
           
            insight = false;
            Vector3 direction = player.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < fieldofviewangle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(eye.transform.position, direction.normalized, out hit, colli.radius))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        insight = true;
                        lastposition.position = player.position;
                        time += Time.deltaTime;
                        Debug.Log(time);
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            insight = false;
        }
    }
}
