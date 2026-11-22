using UnityEngine;
public class MissionIndicatorHandler : MonoBehaviour
{
    public GameObject visual;
    private BaseMission trackingMission;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerMissionHandler.OnMissionAccepted += HandleOnMissionAccepted;
        PlayerMissionHandler.OnMissionCompleted += HandleOnMissionCompleted;
        
    }
    private void OnDisable()
    {
        PlayerMissionHandler.OnMissionAccepted -= HandleOnMissionAccepted;
        PlayerMissionHandler.OnMissionCompleted -= HandleOnMissionCompleted;
    }
    void HandleOnMissionAccepted(BaseMission mission)
    {
        trackingMission = mission;
        visual.gameObject.SetActive(true);
    }
    void HandleOnMissionCompleted(BaseMission mission)
    {
        trackingMission = null;
        visual.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (trackingMission)
        {
            transform.LookAt(trackingMission.Destination, Vector3.up);
        }
    }
}
