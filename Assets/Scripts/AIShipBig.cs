using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class AIShipBig : AIShipBase, IDamagable
{
    #region Variable Declaration
    
    [Header("Mines")]
    public GameObject MinePrefab;
    public float minePlantChance = 0.1f;
    public float minePlantRadius = 5f;
    public float minePlantForce = 5f;
    public int maxMinesInARow = 2;
    public Transform[] minePlantTransforms;

    private int plantedMinesInARow = 0;

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
        agentGraph.GetVariable<float>("MaxDetectionRange", out BlackboardVariable<float> detectionRadiusVar);
        detectionRadius = detectionRadiusVar;
        FindAnyObjectByType<BossHPUI>().enabled = true;
    }

    private void OnDisable()
    {
        GetComponent<HPHandler>().OnDie -= OnDie;
    }
    public override void FireAt(Vector3 targetPosition)
    {
        if (MaximumMinesPlantedInARowReached())
        {
            FireCanons(targetPosition);
            ResetMinesPlantedInARow();
        }
        else
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < minePlantChance)
            {
                PlantMines();
                plantedMinesInARow++;
            }
            else
            {
                FireCanons(targetPosition);
            }
        }
    }

    internal void PlantMines()
    {
        float randomForce = minePlantForce;
        for (int i = 0; i < minePlantTransforms.Length; i++)
        {
            GameObject mine = Instantiate(MinePrefab, minePlantTransforms[i].transform.position, Quaternion.identity);
            mine.GetComponent<MineHandler>().SetInstigator(gameObject);
            randomForce = UnityEngine.Random.Range(0.5f, 1.1f) * minePlantForce;
            mine.GetComponent<Rigidbody>().AddForce(minePlantTransforms[i].transform.forward * randomForce, ForceMode.Impulse);
            
        }
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

    private bool MaximumMinesPlantedInARowReached()
    {
        return plantedMinesInARow >= maxMinesInARow;
    }

    private void ResetMinesPlantedInARow()
    {
        plantedMinesInARow = 0;
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

}
