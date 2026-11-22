using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ShipCanonHandler : BaseCanonHandler
{
    public int index; 
    [SerializeField] public Dictionary<ShipMovementHandler.CanonType, GameObject> canonBallPrefabs;
    public ShipMovementHandler.CanonType CanonType;
    public float aditionalattackPowerCoef = 1;
    public LayerMask waterLayer;
    [SerializeField]
    public Dictionary<ShipMovementHandler.CanonType,AudioClip> shootSounds;
    public InputActionReference InputActionReference;
    public Action<ShipMovementHandler.CanonType, int, float, Vector3> OnShootAction;
    public static Action<float> OnCanonReloadStarted;
    internal bool isReloading = false;
    private Vector3 dir;
    private Vector3 lastClickedPos;
    private AudioSource audioSource;
    private CanonArrangement canonArrangement;
    private void Awake()
    {
        aditionalattackPowerCoef = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        isReloading = false;
    }
    void Start()
    {
        InputActionReference.action.performed += OnShoot;
        canonArrangement = FindAnyObjectByType<CanonArrangement>();
    }
    private void OnDisable()
    {
        InputActionReference.action.performed -= OnShoot;
    }

    public override void OnShoot(InputAction.CallbackContext ctx)
    {
        base.OnShoot(ctx);
        if (canonArrangement.AreCanonsReloading(this))
            return;

        ////Shell Direction
        //Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        //Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        //dir = Vector3.zero;

        //if (Physics.Raycast(ray, out RaycastHit hit, 100f, waterLayer))
        //{
        //    dir = hit.point - transform.position;
        //}
        Invoke(nameof(DoTheActualShooting), (index%3) * 0.1f);
    }

    private void DoTheActualShooting()
    {
        ShootBasedOnCanonType();
        OnShootAction?.Invoke(CanonType, 1, recoilForce, -shootPoint.forward);

        if (muzzleFlashParticleSystem != null)
        {
            Instantiate(muzzleFlashParticleSystem, shootPoint.position, Quaternion.identity);
        }
        StartReload();
    }
    void StartReload()
    {
        isReloading = true;
        float reloadTime = FindAnyObjectByType<CanonArrangement>().CalculateReloadTime(this);
        OnCanonReloadStarted?.Invoke(reloadTime );
        Invoke(nameof(OnReloadDone), reloadTime);
    }

    void OnReloadDone()
    {
        isReloading = false;
    }
    private void OnLevelUpdatedInfo(XPLevelData xpLevelData)
    {
        aditionalattackPowerCoef = xpLevelData.additionalPower;
    }
    private void PlayCanonSound(ShipMovementHandler.CanonType canonType)
    {
        if (shootSounds.ContainsKey(canonType) && shootSounds[canonType] != null)
        {
            audioSource.clip = shootSounds[canonType];
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }
    }

    private void ShootBasedOnStaticVariable()
    {
        if (ShipMovementHandler.currentCanonType != ShipMovementHandler.CanonType.Shotgun)
        {
            GameObject temp_GO;
            CanonBallHandler temp_CanonBall;

            temp_GO = Instantiate(canonBallPrefabs[ShipMovementHandler.currentCanonType], shootPoint.position, Quaternion.identity);
            temp_CanonBall = temp_GO.GetComponent<CanonBallHandler>();
            temp_CanonBall.Initialize(transform.root.gameObject, aditionalattackPowerCoef);
            temp_GO.GetComponent<Rigidbody>().AddForce(shootPoint.forward * temp_CanonBall.shootForce, ForceMode.Impulse);
        }
        switch (ShipMovementHandler.currentCanonType)
        {
            case ShipMovementHandler.CanonType.Normal:

                PlayCanonSound(ShipMovementHandler.CanonType.Normal);
                break;
            case ShipMovementHandler.CanonType.Fire:
                PlayCanonSound(ShipMovementHandler.CanonType.Fire);
                break;
            case ShipMovementHandler.CanonType.Ice:
                PlayCanonSound(ShipMovementHandler.CanonType.Ice);
                break;
            case ShipMovementHandler.CanonType.Shotgun:
                PlayCanonSound(ShipMovementHandler.CanonType.Shotgun);
                for (int i = 0; i < 3; i++)
                {
                    GameObject shotgunCanon = Instantiate(canonBallPrefabs[ShipMovementHandler.currentCanonType], shootPoint.position, Quaternion.identity);
                    Vector3 shootDir = shootPoint.forward + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
                    CanonBallHandler shotgunCanonHandler = shotgunCanon.GetComponent<CanonBallHandler>();
                    shotgunCanonHandler.Initialize(transform.root.gameObject, aditionalattackPowerCoef);
                    shotgunCanon.GetComponent<Rigidbody>().AddForce(shootDir * shotgunCanonHandler.shootForce, ForceMode.Impulse);

                }
                break;

        }
    }
    private void ShootBasedOnCanonType()
    {
        if (CanonType != ShipMovementHandler.CanonType.Shotgun)
        {
            GameObject temp_GO;
            CanonBallHandler temp_CanonBall;

            temp_GO = Instantiate(canonBallPrefabs[CanonType], shootPoint.position, Quaternion.identity);
            temp_CanonBall = temp_GO.GetComponent<CanonBallHandler>();
            temp_CanonBall.Initialize(transform.root.gameObject, aditionalattackPowerCoef);
            temp_GO.GetComponent<Rigidbody>().AddForce(shootPoint.forward * temp_CanonBall.shootForce, ForceMode.Impulse);
        }
        switch (CanonType)
        {
            case ShipMovementHandler.CanonType.Normal:

                PlayCanonSound(ShipMovementHandler.CanonType.Normal);
                break;
            case ShipMovementHandler.CanonType.Fire:
                PlayCanonSound(ShipMovementHandler.CanonType.Fire);
                break;
            case ShipMovementHandler.CanonType.Ice:
                PlayCanonSound(ShipMovementHandler.CanonType.Ice);
                break;
            case ShipMovementHandler.CanonType.Shotgun:
                PlayCanonSound(ShipMovementHandler.CanonType.Shotgun);
                for (int i = 0; i < 3; i++)
                {
                    GameObject shotgunCanon = Instantiate(canonBallPrefabs[CanonType], shootPoint.position, Quaternion.identity);
                    Vector3 shootDir = shootPoint.forward + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
                    CanonBallHandler shotgunCanonHandler = shotgunCanon.GetComponent<CanonBallHandler>();
                    shotgunCanonHandler.Initialize(transform.root.gameObject, aditionalattackPowerCoef);
                    shotgunCanon.GetComponent<Rigidbody>().AddForce(shootDir * shotgunCanonHandler.shootForce, ForceMode.Impulse);

                }
                break;

        }
    }
}
