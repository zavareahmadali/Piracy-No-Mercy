using UnityEngine;

public class BaseMission : MonoBehaviour
{
    public string MissionID;
    public int XPReward;
    public int GoldReward;
    public enum MissionType { Cargo, Escort, Rescue}
    public MissionType missionType;
    public Transform Destination;
    public virtual void StartMission()
    {
        Debug.Log($"Mission {MissionID} started.");
    }
    public virtual void CompleteMission()
    {
        Debug.Log($"Mission {MissionID} completed. Rewards: {XPReward} XP, {GoldReward} Gold.");
    }
}
