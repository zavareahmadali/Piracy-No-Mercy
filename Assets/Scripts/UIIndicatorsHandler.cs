using UnityEngine;
using UnityEngine.UI;
public class UIIndicatorsHandler : MonoBehaviour
{

    public Image TopIndicator;
    public Image BottomIndicator;
    public Image LeftIndicator;
    public Image RightIndicator;
    public Image MissionTracker;
    private BaseMission trackingMission;
    Vector3 tempPos;
    void Start()
    {
        AIShipBase.OnShootAction += HandleShootAction;
        //PlayerMissionHandler.OnMissionAccepted += HandleMissionUpdate;
    }
    private void OnDisable()
    {
        AIShipBase.OnShootAction -= HandleShootAction;
        //PlayerMissionHandler.OnMissionAccepted -= HandleMissionUpdate;
    }
    private void HandleShootAction(Vector3 position)
    {
        // Handle the shoot action, e.g., display an indicator at the position
        Vector3 viewPos = Camera.main.WorldToViewportPoint(position);
        if (viewPos.x < 0)
        {
            LeftIndicator.gameObject.SetActive(true);
            LeftIndicator.color = new Color(LeftIndicator.color.r, LeftIndicator.color.g, LeftIndicator.color.b, 0.25f);
            Invoke(nameof(HideIndicators), 0.5f);
        }
        else if (viewPos.x > 1)
        {
            RightIndicator.gameObject.SetActive(true);
            RightIndicator.color = new Color(RightIndicator.color.r, RightIndicator.color.g, RightIndicator.color.b, 0.25f);
            Invoke(nameof(HideIndicators), 0.5f);
        }
        else if (viewPos.y < 0)
        {
            BottomIndicator.gameObject.SetActive(true);
            BottomIndicator.color = new Color(BottomIndicator.color.r, BottomIndicator.color.g, BottomIndicator.color.b, 0.25f);
            Invoke(nameof(HideIndicators), 0.5f);
        }
        else if (viewPos.y > 1)
        {
            TopIndicator.gameObject.SetActive(true);
            TopIndicator.color = new Color(TopIndicator.color.r, TopIndicator.color.g, TopIndicator.color.b, 0.25f);
            Invoke(nameof(HideIndicators), 0.5f);
        }

    }

    private void HandleMissionUpdate(BaseMission mission)
    {
        MissionTracker.gameObject.SetActive(true);
        trackingMission = mission;
    }
    void HideIndicators()
    {
        LeftIndicator.gameObject.SetActive(false);
        RightIndicator.gameObject.SetActive(false);
        TopIndicator.gameObject.SetActive(false);
        BottomIndicator.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (trackingMission!= null)
        {
            tempPos = Camera.main.WorldToViewportPoint(trackingMission.Destination.position);
            if (tempPos.x < 0 || tempPos.x > 1 || tempPos.y < 0 || tempPos.y > 1)
            {
                if (!MissionTracker.gameObject.activeInHierarchy)
                {
                    MissionTracker.gameObject.SetActive(true); 
                }
                MissionTracker.rectTransform.anchoredPosition = new Vector3( (1000/2) * Camera.main.WorldToViewportPoint(trackingMission.Destination.position).x,
                    (1080 / 2) * Camera.main.WorldToViewportPoint(trackingMission.Destination.position).y, 0);
                Debug.Log(tempPos);
            }
            else
            {
                MissionTracker.gameObject.SetActive(false);
            }
        }
    }
}
