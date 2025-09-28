using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Walk : MoveTo
{
    protected NavMeshAgent agent
    {
        get
        {
            _agent ??= GetComponent<NavMeshAgent>();
            return _agent;
        }
    }

    NavMeshAgent _agent;

    public override void Goto(Vector3 destination)
    {
        base.Goto(destination);
        agent.SetDestination(destination);
        agent.isStopped = false;
    }

    protected override bool AtDestination()
    {
        if (!agent) return false;
        if (agent.enabled == false) return false;
        if (!agent.hasPath) return false;
        if (agent.isStopped) return false;
        if (agent.pathPending) return false;
        if (agent.isPathStale) return false;

        return agent?.remainingDistance < 0.1f;
    }
}