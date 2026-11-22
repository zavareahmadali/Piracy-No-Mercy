using System;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioHandler))]
public class XPHandler : MonoBehaviour
{
    public int currentXP;
    public int currentLevel = 0;
    public int currentCanonPoints = 0;
    public static XPHandler instance;
    public GameObject levelUpEffect;

    public XPLevelData[] xpLevelData;
    public static Action<int, int, int, int, int> OnXPUpdated;
    public static Action<int> OnXPAdded;
    public static Action<int> OnLevelUpdated;
    public static Action<int> OnCanonPointUpdated;
    public static Action<XPLevelData> OnLevelUpdatedInfo;

    private AudioHandler audioHandler;


    private void Awake()
    {
        instance = this;
        audioHandler = GetComponent<AudioHandler>();
    }

    void Start()
    {
        currentXP = 0;
        currentCanonPoints = 0;
        UpdateXP(currentXP);
    }

    public void SpendCanonPoint(int canonTypeIndex)
    {
        if (currentCanonPoints <= 0)
            return;
        currentCanonPoints--;
        OnCanonPointUpdated?.Invoke(currentCanonPoints);
    }

    public int GetCurrentCanonPoints()
    {
        return currentCanonPoints;
    }

    public static void GetXP(int xp)
    {
        instance.currentXP += xp;
        UpdateXP(xp);

        if (instance.currentLevel == instance.xpLevelData.Length - 1)
            return;
        
        if (instance.currentXP < instance.xpLevelData[instance.currentLevel].xpRequired)
            return;

        instance.currentLevel++;
        instance.currentCanonPoints++;
        instance.currentLevel = Mathf.Clamp(instance.currentLevel, 0, instance.xpLevelData.Length - 1);
        
        OnLevelUpdated?.Invoke(instance.currentLevel);
        OnLevelUpdatedInfo?.Invoke(instance.xpLevelData[instance.currentLevel]);
        OnCanonPointUpdated?.Invoke(instance.currentCanonPoints);
        
        instance.audioHandler.PlaySFX(SFXType.LevelUp);
        
        UpdateXP(0);
        Instantiate(instance.levelUpEffect, instance.transform.position, Quaternion.identity);

    }

    public static void UpdateXP(int xp)
    {
        OnXPUpdated?.Invoke(
            xp,
            instance.currentXP,
            (instance.currentLevel == 0) ? 0 : instance.xpLevelData[Mathf.Clamp(instance.currentLevel - 1, 0, instance.xpLevelData.Length - 1)].xpRequired,
            instance.xpLevelData[instance.currentLevel].xpRequired,
            instance.currentLevel);

        OnXPAdded?.Invoke(xp);
    }
    
}

public class XPConstants
{
    public const int XP_PER_ENEMY_DAMAGED = 2;
    public const int XP_PER_ENEMY_KILLED = 3;
    public const int XP_PER_DAMAGE_TAKEN = 1;
    public const int XP_PER_BOSS = 5;
}

[Serializable]
public class XPLevelData
{
    public int xpRequired;
    [InspectorLabel("+HP")] public float additionalHP;
    [InspectorLabel("+Power")] public float additionalPower;
    [InspectorLabel("-Reload")] public float reducedReloadTime;
    [InspectorLabel("MoveSpeed")] public float movementSpeed;
    [InspectorLabel("RotSpeed")] public float rotationSpeed;

}