using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolInCircle", story: "[Agent] patrols in [radius]", category: "Action", id: "5653481b2655e72bf8bb97df78e99737")]
public partial class PatrolInCircleAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Radius;

    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> Speed = new(3f);
    [SerializeReference] public BlackboardVariable<float> WaypointWaitTime = new(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new("SpeedMagnitude");
    [Tooltip("Should patrol restart from the latest point?")]
    [SerializeReference] public BlackboardVariable<bool> PreserveLatestPatrolPoint = new(false);

    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    [CreateProperty] private Vector3 m_CurrentTarget;
    [CreateProperty] private float m_OriginalStoppingDistance = -1f;
    [CreateProperty] private float m_OriginalSpeed = -1f;
    [CreateProperty] private float m_WaypointWaitTimer;
    [CreateProperty] private int m_CurrentPatrolPoint = 0;
    [CreateProperty] private bool m_Waiting;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }

        Initialize();

        m_Waiting = false;
        m_WaypointWaitTimer = 0.0f;

        MoveToNextWaypoint();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null)
        {
            return Status.Failure;
        }

        if (m_Waiting)
        {
            if (m_WaypointWaitTimer > 0.0f)
            {
                m_WaypointWaitTimer -= Time.deltaTime;
            }
            else
            {
                m_WaypointWaitTimer = 0f;
                m_Waiting = false;
                MoveToNextWaypoint();
            }
        }
        else
        {
            float distance = GetDistanceToWaypoint();
            bool destinationReached = distance <= DistanceThreshold;

            // Check if we've reached the waypoint (ensuring NavMeshAgent has completed path calculation if available)
            if (destinationReached && (m_NavMeshAgent == null || !m_NavMeshAgent.pathPending))
            {
                m_WaypointWaitTimer = WaypointWaitTime.Value;
                m_Waiting = true;

                return Status.Running;
            }
        }


        return Status.Running;
    }

    protected override void OnEnd()
    {

        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.speed = m_OriginalSpeed;
            m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;
        }
    }

    protected override void OnDeserialize()
    {
        // If using a navigation mesh, we need to reset default value before Initialize.
        m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_OriginalSpeed >= 0f)
                m_NavMeshAgent.speed = m_OriginalSpeed;
            if (m_OriginalStoppingDistance >= 0f)
                m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;

            m_NavMeshAgent.Warp(Agent.Value.transform.position);
        }

        int patrolPoint = m_CurrentPatrolPoint - 1;
        Initialize();
        // During deserialization, bypass PreserveLatestPatrolPoint.
        m_CurrentPatrolPoint = patrolPoint;
    }

    private void Initialize()
    {
        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        m_NavMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }

            m_OriginalSpeed = m_NavMeshAgent.speed;
            m_NavMeshAgent.speed = Speed.Value;
            m_OriginalStoppingDistance = m_NavMeshAgent.stoppingDistance;
            m_NavMeshAgent.stoppingDistance = DistanceThreshold;
        }

        m_CurrentPatrolPoint = PreserveLatestPatrolPoint.Value ? m_CurrentPatrolPoint - 1 : -1;

    }

    private float GetDistanceToWaypoint()
    {
        if (m_NavMeshAgent != null)
        {
            return m_NavMeshAgent.remainingDistance;
        }

        Vector3 targetPosition = m_CurrentTarget;
        Vector3 agentPosition = Agent.Value.transform.position;
        agentPosition.y = targetPosition.y; // Ignore y for distance check.
        return Vector3.Distance(agentPosition, targetPosition);
    }

    private void MoveToNextWaypoint()
    {

        m_CurrentTarget = Agent.Value.transform.position + UnityEngine.Random.insideUnitSphere * Radius.Value;
        m_CurrentTarget.y = Agent.Value.transform.position.y; // Keep the same height

        if (m_NavMeshAgent != null)
        {
            m_NavMeshAgent.SetDestination(m_CurrentTarget);
        }
    }

}


