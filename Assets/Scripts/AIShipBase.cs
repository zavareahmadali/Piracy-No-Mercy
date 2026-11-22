using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class AIShipBase : MonoBehaviour
{
    #region Variable Declaration

    [Header("Canon")]
    public GameObject CanonBall;
    public Transform ForwardCanonTransform;
    public float FireForce = 1000f;

    [Header("Effects")]
    public GameObject ExplosionPrefab;
    public GameObject IceEffectPrefab;
    public float FreezeDuration = 5f;
    public float FreezeSpeedMultiplier = 0.5f;

    [Header("Loot Chance")]
    public List<LootChanceTable> LootChances;
    internal NavMeshAgent agent;
    internal BehaviorGraphAgent agentGraph;
    internal Blackboard blackboard;
    internal bool hasReachedDestination = false;
    public Action<bool, float> TakeDamageAction;

    internal float detectionRadius = 10f;
    internal float speedBeforeFreeze = 0f;
    internal bool isFrozen = false;
    public static Action<Vector3> OnShootAction;
    internal AudioHandler audioHandler;
    internal Vector2 waveID;
    
    #endregion

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

        agentGraph.Init();
        agentGraph.GetVariable<float>("DetectionRange", out BlackboardVariable<float> detectionRadiusVar);
        detectionRadius = detectionRadiusVar;
    }

    private void OnDisable()
    {
        GetComponent<HPHandler>().OnDie -= OnDie;
    }
    public virtual void FireAt(Vector3 targetPosition)
    {
        FireCanons(targetPosition);
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
        GameObject canonBall = Instantiate(CanonBall, ForwardCanonTransform.position + new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f)), Quaternion.identity);
        canonBall.GetComponent<Rigidbody>().AddForce(ForwardCanonTransform.forward * FireForce);
        canonBall.GetComponent<CanonBallHandler>().Initialize(transform.root.gameObject, 0);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    public bool HasReachedDestination()
    {
        hasReachedDestination = Vector3.Distance(transform.position, agent.destination) < 1f;
        return hasReachedDestination;
    }

    public void TakeDamage(GameObject instigator, float damage, ShipMovementHandler.CanonType damageType, Vector3 hitPoint, Vector3 hitDirection)
    {
        bool isPlayers = false;
        if (instigator.TryGetComponent<CanonBallHandler>(out CanonBallHandler canon))
        {
            canon.instigator.TryGetComponent<ShipMovementHandler>(out ShipMovementHandler shipMovement);
            isPlayers = shipMovement != null;
        }
        else if (instigator.TryGetComponent<ShipMovementHandler>(out ShipMovementHandler shipMovement))
            isPlayers = true;
        if (damageType == ShipMovementHandler.CanonType.Ice)
        {
            SetStateToFrozen();

        }
        TakeDamageAction?.Invoke(isPlayers, damage);
        audioHandler.PlaySFX(SFXType.Hit);
    }

    public void OnDie()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        agentGraph.End();
        agentGraph.enabled = false;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;

        LootManager.GiveLoot(transform.position, LootChances);
        audioHandler.PlaySFX(SFXType.Explosion);
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
            return;

        if (collision.transform.TryGetComponent<IDamagable>(out IDamagable d))
        {
            float damage = 1;

            if (collision.gameObject.TryGetComponent<MineHandler>(out MineHandler mine))
            {
                if (mine == null)
                    d.TakeDamage(gameObject, damage, ShipMovementHandler.CanonType.Normal, collision.contacts[0].point, (transform.position - collision.contacts[0].point).normalized);

                if (mine.instigator == this.gameObject)
                    return;

                else
                    d.TakeDamage(gameObject, damage, ShipMovementHandler.CanonType.Normal, collision.contacts[0].point, (transform.position - collision.contacts[0].point).normalized);
            }

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void SetStateToFrozen()
    {
        if (isFrozen)
            return;

        isFrozen = true;
        IceEffectPrefab.SetActive(true);
        speedBeforeFreeze = agent.speed;
        agent.speed = FreezeSpeedMultiplier * speedBeforeFreeze;
        Invoke(nameof(SetStateToNormal), FreezeDuration);
    }

    public void SetStateToNormal()
    {
        IceEffectPrefab.SetActive(false);
        agent.speed = speedBeforeFreeze;
        isFrozen = false;
    }
}
