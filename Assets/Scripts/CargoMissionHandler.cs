using System;
using UnityEngine;

public class CargoMissionHandler : BaseMission
{
    public GameObject missionVFX;

    [Header("Destination Settings")]
    public Transform cargoDropoffPoint;
    [SerializeField] private Transform rescueeDropOffPoint;
    [SerializeField] private Transform rescueeDropOffPointVFX;
    [SerializeField] private Transform heartVFXSpawnPosition;

    [Header("Rescuee Settings")]
    [SerializeField] private GameObject rescuee;
    private Animator rescueeAnimator;
    [SerializeField] private float greetingFrequencyInSeconds;
    [SerializeField] private float hideRescueeAfterSeconds; 
    private bool isMissionActive = false;
    
    void Start()
    {
        rescueeAnimator = rescuee.GetComponent<Animator>();
        InvokeRepeating(nameof(GreetPlayer), 5, greetingFrequencyInSeconds);
    }

    public override void StartMission()
    {
        base.StartMission();
        HideResucee();
        isMissionActive = true;
        Destination.gameObject.SetActive(true);
        missionVFX.gameObject.SetActive(false);
        Debug.Log($"Cargo mission {MissionID} started. Pickup at {transform.position}, dropoff at {cargoDropoffPoint.position}.");
    }

    public override void CompleteMission()
    {
        base.CompleteMission();
        XPHandler.GetXP(XPReward);
        PlayerLootHandler.AddGold(GoldReward);
        Destination.gameObject.SetActive(false);
        rescuee.transform.position = rescueeDropOffPoint.transform.position;
        rescuee.gameObject.SetActive(true);
        rescueeAnimator.SetBool(UnityEngine.Random.value < 0.5f ? "Dance 1" : "Dance 2", true);
        Invoke(nameof(HideResucee), 10);
        Instantiate(rescueeDropOffPointVFX, heartVFXSpawnPosition.position, rescueeDropOffPointVFX.rotation);
    }

    private void OnDrawGizmos()
    {
        if (isMissionActive)
        {
            if (cargoDropoffPoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(cargoDropoffPoint.position, 1f);
            }
        }
    }
    
    private void HideResucee()
    {
        rescuee.SetActive(false);

    }
    private void GreetPlayer()
    {
        rescueeAnimator.SetTrigger("Greeting");
        Vector3 lookPosition = FindAnyObjectByType<ShipMovementHandler>().transform.position;
        lookPosition.y = rescuee.transform.position.y;
        rescuee.transform.LookAt(lookPosition, Vector3.up);
    }
}
