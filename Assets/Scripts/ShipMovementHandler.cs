using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.InputSystem;

public class ShipMovementHandler : MonoBehaviour, IDamagable
{
    
    #region Variable Declaration
    [Header("Movement Settings")]
    [SerializeField] internal float speed = 5f;
    [SerializeField] internal float rotationSpeed = 1f;
    
    [Header("Collision Settings")]
    [SerializeField] private float maxCollisionDamage = 100f;
    [SerializeField] internal float minimumCollisionSpeed = 5f;
    [SerializeField] internal float maxCollisionSpeed = 40f;

    [Header("Boost Settings")]
    [SerializeField] private float boostSpeed = 10f;
    [SerializeField] private float boostDuration = 2f;

    [Header("Canon Settings")]
    public static CanonType currentCanonType;
    public static float attackMultiplier = 1f;

    private PlayerInput playerInput;
    private Vector2 movementInput;
    private Rigidbody rb;
    public enum CanonType { Normal = 0, Fire = 1, Shotgun = 2, Ice = 3, None = -1 }
    private ShipCanonHandler[] canons;
    private PlayerHPHandler playerHPHandler;
    private bool isBoosting = false;
    private AudioHandler audioHandler;

    public static Action<ShipMovementHandler.CanonType> OnShellTypeChanged;
    public static Action<float> OnDamaged;
    

    #endregion

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerHPHandler = GetComponent<PlayerHPHandler>();
        rb = GetComponent<Rigidbody>();
        audioHandler = GetComponent<AudioHandler>();
        rb.inertiaTensor = new Vector3(1, 1, 1);
        canons = GetComponentsInChildren<ShipCanonHandler>();
        attackMultiplier = 1f;
        isBoosting = false;
    }
    
    private void Start()
    {
        AnchorHandler.OnPerfectHook += OnBoost;
        currentCanonType = CanonType.Normal;
        foreach (var canon in canons)
        {
            canon.OnShootAction += AddRecoilToShip;
        }
    }
    
    private void OnDisable()
    {
        AnchorHandler.OnPerfectHook -= OnBoost;
    }

    private void AddRecoilToShip(CanonType canonType, int count, float amount, Vector3 direction)
    {
        rb.AddForce(amount * direction);
    }

    private void FixedUpdate()
    {
        rb.AddForce((!isBoosting ? speed : boostSpeed) * movementInput.y * transform.forward);
        rb.AddTorque(-rotationSpeed * movementInput.x * Vector3.up);
        if (rb.linearVelocity.magnitude >= (!isBoosting ? speed : boostSpeed))
        {
            rb.linearVelocity = (!isBoosting ? speed : boostSpeed) * rb.linearVelocity.normalized;
        }

    }

    private void OnMove()
    {
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void OnChangeCanonType()
    {
        currentCanonType++;
        if ((int)currentCanonType >= Enum.GetNames(typeof(CanonType)).Length)
        {
            currentCanonType = CanonType.Normal;
        }
        OnShellTypeChanged?.Invoke(currentCanonType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CanonBallHandler>(out CanonBallHandler c))
        {
            if (c.instigator != this.gameObject)
            {
                TakeDamage(c.gameObject, c.attackPower, c.canonType, other.transform.position, other.transform.position - transform.position);
                c.GetDestroyed();
            }
        }
        else if (other.transform.TryGetComponent<IDamagable>(out IDamagable d))
        {
            d.TakeDamage(gameObject, 100, CanonType.Normal, other.transform.position, (transform.position - other.transform.position).normalized);
        }
        else if (other.CompareTag("Loot"))
        {
            other.GetComponent<LootHandler>().LootAreaEntered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Loot"))
            other.GetComponent<LootHandler>().LootAreaExited();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<IDamagable>(out IDamagable d))
        {
            float damage = CalculateCollisionDamage(collision.relativeVelocity.magnitude);

            if (damage == 0)
                return;

            d.TakeDamage(gameObject, damage, CanonType.Normal, collision.contacts[0].point, (transform.position - collision.contacts[0].point).normalized);
        }
        else if (collision.gameObject.CompareTag("Mine"))
            collision.gameObject.GetComponent<MineHandler>().OnExplode();
    }
    
    public void TakeDamage(GameObject instigator, float damage, ShipMovementHandler.CanonType damageType, Vector3 hitPoint, Vector3 hitDirection)
    {
        playerHPHandler.GetDamage(damage);
        XPHandler.GetXP(XPConstants.XP_PER_DAMAGE_TAKEN);
        OnDamaged?.Invoke(damage);
        audioHandler.PlaySFX(SFXType.Hit);
    }

    private float CalculateCollisionDamage(float realtiveSpeed)
    {
        return (realtiveSpeed >= minimumCollisionSpeed) ? Mathf.Clamp(realtiveSpeed / maxCollisionSpeed * maxCollisionDamage, 0, maxCollisionDamage) : 0;
    }

    private void OnLevelUpdatedInfo(XPLevelData xpleveldata)
    {
        speed = xpleveldata.movementSpeed;
        rotationSpeed = xpleveldata.rotationSpeed;
    }

    public static void ChangeAttackMultiplier(float newValue)
    {
        attackMultiplier = newValue;
    }

    public void OnBoost()
    {
        StartCoroutine(DoBoost());
    }

    private IEnumerator DoBoost()
    {
        float timer = boostDuration;
        isBoosting = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        isBoosting = false;
        yield return null;
    }
}
