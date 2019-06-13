using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine.AI;

public class CustomNavMeshAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    public bool isStopped { get { return agent.isStopped; } set { agent.isStopped = value; } }
    public bool hasPath { get { return agent.hasPath; } }
    public Vector3 destination { get { return agent.destination; } set { agent.destination = value; } }

    private bool isMoving = false;
    private Task endOfMovementEvent;
    private float pathEndThreshold = 0.1f;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void FixedUpdate()
    {
        if (isMoving && agent.pathPending == false && agent.remainingDistance <= agent.stoppingDistance + pathEndThreshold)
        {
            isMoving = false;
            endOfMovementEvent.Start();
        }
    }

    public async Task SetDestination(Vector3 position)
    {
        isMoving = true;
        agent.destination = position;
        agent.isStopped = false;
        
        endOfMovementEvent = new Task(() => {
            UnityThread.executeInFixedUpdate(() =>
            {
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
            });
        });

        await endOfMovementEvent;
    }
}