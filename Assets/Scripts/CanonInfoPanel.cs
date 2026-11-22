using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
public class CanonInfoPanel : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text description;

    [SerializeField]
    public Dictionary<ShipMovementHandler.CanonType, CanonInfo> canonInfos;
    public static ShipMovementHandler.CanonType selectedCanonType;
    void Start()
    {
        selectedCanonType = ShipMovementHandler.CanonType.Normal;
        ResetPanel();
    }

    void ResetPanel()
    {
        icon.enabled = false;
        title.text = "";
        description.text = "";
    }
    public void SetCanonInfo(int canonTypeIndex)
    {
        if (!icon.enabled)
            icon.enabled = true;

        if (canonInfos.ContainsKey((ShipMovementHandler.CanonType)canonTypeIndex))
        {
            CanonInfo info = canonInfos[(ShipMovementHandler.CanonType)canonTypeIndex];
            icon.sprite = info.icon;
            title.text = info.title;
            description.text = info.description;
        }
        selectedCanonType = (ShipMovementHandler.CanonType)canonTypeIndex;
    }
}
[Serializable]
public struct CanonInfo
{
    public Sprite icon;
    public string title;
    public string description;
    public CanonInfo(Sprite icon, string title, string description)
    {
        this.icon = icon;
        this.title = title;
        this.description = description;
    }
}