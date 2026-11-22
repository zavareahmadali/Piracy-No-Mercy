using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MineHandler : MonoBehaviour, IDamagable
{
    public GameObject ExplosionEffect;
    public bool isBossMine = false;
    public float explosionDelay = 3f;
    public float armingDuration;// Delay before the mine explodes
    public float damage = 5;
    public float damageRadius = 10;
    public AnimationCurve damageDecayCurve;
    public LayerMask damageLayers;
    internal GameObject instigator;
    private bool isGoingToExplode = false;
    private Rigidbody rb;
    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isBossMine)
        {
            yield return new WaitForSeconds(armingDuration);
            StartCoroutine(nameof(Blink));
            Invoke(nameof(OnExplode), explosionDelay);
            yield return null;
        }
    }

    public void SetInstigator(GameObject instigator)
    {
        this.instigator = instigator;
    }

    public void TakeDamage(GameObject source, float damage, ShipMovementHandler.CanonType damageType, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!isGoingToExplode)
        {
            isGoingToExplode = true;
            Invoke(nameof(OnExplode), 0.1f); // Delay the explosion slightly
        }
    }

    public void OnExplode()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, damageRadius, Vector3.up);

        foreach (var hit in hits)
        {
            if (isBossMine && hit.collider.GetComponent<AIShipBase>())
                continue;


            if (hit.transform.TryGetComponent<IDamagable>(out IDamagable d))
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                float damageMultiplier = damageDecayCurve.Evaluate(Mathf.Clamp(damageRadius - distance, 0, damageRadius) / damageRadius);
                d.TakeDamage(gameObject, damage * damageMultiplier, ShipMovementHandler.CanonType.Normal, hit.point, (transform.position - hit.point).normalized);
            }
        }

        GameObject explosionEffect = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        if (explosionEffect.TryGetComponent<AudioHandler>(out AudioHandler audioH))
        {
            audioH.PlaySFX(SFXType.Explosion); 
        }
        Destroy(gameObject);
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnExplode();
    }

    private IEnumerator Blink()
    {
        float blinkDuration = 0.25f; 
        float totalDuration = 3f; 
        float elapsedTime = 0f;
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        Color originalColor = renderer.material.color;
        while (elapsedTime < totalDuration)
        {
            foreach (var m in materials)
            {
                m.color = Color.red; 
            }
            yield return new WaitForSeconds(blinkDuration);
            foreach (var m in materials)
            {
                m.color = originalColor; 
            }
            yield return new WaitForSeconds(blinkDuration);
            elapsedTime += blinkDuration * 2;
        }
        OnExplode();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

        }
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;
        if (transform.position.y > 1f)
            return;
        

        rb.useGravity = false;
        rb.isKinematic = true;

    }

}
