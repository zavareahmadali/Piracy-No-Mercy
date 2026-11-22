using System.Collections;
using UnityEngine;

public class CanonBallHandler : MonoBehaviour
{
    public ShipMovementHandler.CanonType canonType;
    public float attackPower = 1;
    public float shootForce = 100f;
    private float initialAttackPowerCoef = 1;
    private float attackPowerCoefficent = 1;

    public float reloadTime = 1f;
    public GameObject HitEffect;
    internal GameObject instigator;
    private TrailRenderer trail;
    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        initialAttackPowerCoef = attackPower;
        StartCoroutine(nameof(DisableTrail));
    }
    internal void Initialize(GameObject instigator, float attackPowerCoef)
    {
        this.instigator = instigator;
        attackPowerCoefficent = initialAttackPowerCoef + attackPowerCoef;
        GetComponent<Collider>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.transform.CompareTag(TagsAndConsts.Tags.PLAYER) && transform.CompareTag(TagsAndConsts.Tags.PLAYERMADE))
        //    return;
        if (other.gameObject == instigator)
            return;
        if (other.GetComponent<CanonBallHandler>())
            return;
        if (other.transform.CompareTag(instigator.transform.tag))
            return;
        

        if (other.transform.TryGetComponent<IDamagable>(out IDamagable d) && other.gameObject != instigator)
        {
            Instantiate(HitEffect, other.ClosestPointOnBounds(transform.position), Quaternion.LookRotation(other.transform.forward));
            d.TakeDamage(gameObject, attackPower * attackPowerCoefficent, canonType, other.transform.position, (transform.position - other.transform.position).normalized);
            GetDestroyed();
        }
        else
        {
            if (other.CompareTag("Loot"))
                return;
            if (other.CompareTag("Region"))
                return;

            Instantiate(HitEffect, other.ClosestPointOnBounds(transform.position), Quaternion.LookRotation(other.transform.forward));
            GetDestroyed();
        }
    }

    public void GetDestroyed()
    {
        Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public IEnumerator DisableTrail()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            if (transform.position.y > 1)
            {
                continue;
            }

            if (trail != null)
                trail.enabled = false;

            break;
        }
        yield return null;
    }

}
