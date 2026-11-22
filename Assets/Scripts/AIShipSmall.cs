using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class AIShipSmall : AIShipBase, IDamagable
{
    public float spreadRadius = 0.5f;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agentGraph = GetComponent<BehaviorGraphAgent>();
        blackboard = agentGraph.BlackboardReference.Blackboard;
        
        audioHandler = GetComponent<AudioHandler>();
    }

    void Start()
    {
        GetComponent<HPHandler>().OnDie += OnDie;
        agentGraph.SetVariableValue<ShipMovementHandler>("Player", FindAnyObjectByType<ShipMovementHandler>());
        agentGraph.Init();
        agentGraph.GetVariable<float>("DetectionRange", out BlackboardVariable<float> detectionRadiusVar);

        detectionRadius = detectionRadiusVar;
    }

    private void OnDisable()
    {
        GetComponent<HPHandler>().OnDie -= OnDie;
    }
    public override void FireAt(Vector3 targetPosition)
    {
        CreateAndInitializeCanonBall();

        OnShootAction?.Invoke(transform.position);
        audioHandler.PlaySFX(SFXType.Shoot);
    }

    private void FireCanons(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        for (int i = 0; i < 5; i++)
        {
            CreateAndInitializeCanonBall();
        }
        OnShootAction?.Invoke(transform.position);
        audioHandler.PlaySFX(SFXType.Shoot);
    }

    private void CreateAndInitializeCanonBall()
    {
        GameObject canonBall = Instantiate(CanonBall, ForwardCanonTransform.position, Quaternion.identity);
        Vector3 randomDir = (ForwardCanonTransform.forward + UnityEngine.Random.insideUnitSphere * spreadRadius).normalized;

        canonBall.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.Range(0.7f, 1f) * FireForce * randomDir);
        canonBall.GetComponent<CanonBallHandler>().Initialize(transform.root.gameObject, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
