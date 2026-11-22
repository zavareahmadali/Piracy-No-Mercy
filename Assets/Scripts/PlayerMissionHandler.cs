using System;
using UnityEngine;

public class PlayerMissionHandler : MonoBehaviour
{
    public bool hasAMission;
    private BaseMission currentMission;
    public static Action<BaseMission> OnMissionAccepted;
    public static Action<BaseMission> OnMissionCompleted;
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasAMission)
        {
            if (other.CompareTag(TagsAndConsts.Tags.MISSION))
            {
                BaseMission mission = other.GetComponent<BaseMission>();
                if (mission != null)
                {
                    mission.StartMission();
                    OnMissionAccepted?.Invoke(mission);
                    hasAMission = true;
                    currentMission = mission;
                }
            }
        }
        else
        {
            if (other.CompareTag(TagsAndConsts.Tags.MISSIONDESTINATION))
            {
                currentMission.CompleteMission();
                OnMissionCompleted?.Invoke(currentMission);
                currentMission = null;
                hasAMission = false;
            }
        }

    }
}
