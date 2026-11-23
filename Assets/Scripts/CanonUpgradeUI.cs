using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
public class CanonUpgradeUI : MonoBehaviour
{
    public GameObject MiniUpgradeUI;
    public GameObject NotSpentPointsAvailableUI;
    public TMP_Text AvailableCanonPointsText;
    public Animator CanonUpgradeAnimator;
    public CanonUpgradeBuyButton canonUpgradeBuyButton;
    public GameObject selectionIndicator;
    private int selectedCanonPositionIndex;
    private int selectedCanonTypeIndex;
    public GameObject[] canonPositionIcons;
    public GameObject[] canonMiniPositions;

    private List<Image> canonPositionIconsImageComp;
    [SerializeField]
    public Dictionary<ShipMovementHandler.CanonType, Sprite> CanonIcons;
    private const string SHOWCANONUIPARAM = "Show";
    private AudioHandler audioHandler;

    private void Awake()
    {
        audioHandler = GetComponent<AudioHandler>();
    }
    void Start()
    {
        selectedCanonPositionIndex = -1;
        selectedCanonTypeIndex = (int)ShipMovementHandler.CanonType.None;
        XPHandler.OnCanonPointUpdated += OnCanonPointsUpdated;

        canonPositionIconsImageComp = new List<Image>(canonPositionIcons.Length);
        for (int i = 0; i < canonPositionIcons.Length; i++)
            canonPositionIconsImageComp.Add(canonPositionIcons[i].GetComponent<Image>());

        CanonArrangement.OnCanonArrangmentChanged += UpdateMiniCanons;
        CanonArrangement.OnCanonArrangmentChanged += UpdateCanonIconPositions;
    }

    private void OnDisable()
    {
        XPHandler.OnCanonPointUpdated -= OnCanonPointsUpdated;
        CanonArrangement.OnCanonArrangmentChanged -= UpdateMiniCanons;
        CanonArrangement.OnCanonArrangmentChanged -= UpdateCanonIconPositions;
    }

    public void OnCanonPointsUpdated(int canonPoints)
    {
        NotSpentPointsAvailableUI.SetActive(canonPoints != 0);
        AvailableCanonPointsText.text = canonPoints.ToString();
    }

    public void OnMiniUpgradeUIClicked()
    {
        TogglePanel(true);
    }

    public void OnCloseButtonClicked()
    {
        TogglePanel(false);
        
    }
    
    private void TogglePanel(bool isOpening)
    {
        if (GameManagerHandler.GetIsAnyOverlayScreenActive())
            return;

        MiniUpgradeUI.SetActive(!isOpening);
        CanonUpgradeAnimator.SetBool(SHOWCANONUIPARAM, isOpening);
        if (isOpening)
        {
            GameManagerHandler.PauseGame();
            audioHandler.PlaySFX(SFXType.Open);
        }
        else
        {
            GameManagerHandler.ResumeGame();
            audioHandler.PlaySFX(SFXType.Close);
        }
    }
    
    public void OnCanonTypeClicked(int index)
    {
        selectedCanonTypeIndex = index;
        CheckIfBuyButtonShouldBeInteractable();
        audioHandler.PlaySFX(SFXType.Select);
    }

    public void OnCanonIconPositionClicked(int index)
    {
        selectedCanonPositionIndex = index;
        selectionIndicator.gameObject.SetActive(true);

        selectionIndicator.transform.position = canonPositionIcons[index].transform.position;

        CheckIfBuyButtonShouldBeInteractable();
        audioHandler.PlaySFX(SFXType.Select);
    }

    public void OnCanonUpgradeBuyButtonClicked()
    {
        
        XPHandler.instance.SpendCanonPoint(selectedCanonPositionIndex);
        FindAnyObjectByType<CanonArrangement>().SetCanonArrangement(selectedCanonPositionIndex, (ShipMovementHandler.CanonType)selectedCanonTypeIndex);
        
        selectionIndicator.SetActive(false);
        canonUpgradeBuyButton.SetInteractableState(false);

        if (XPHandler.instance.GetCurrentCanonPoints() == 0)
        {
            TogglePanel(false);
        }
        audioHandler.PlaySFX(SFXType.Bought);
    }

    public void UpdateMiniCanons(List<ShipMovementHandler.CanonType> canons)
    {
        for (int i = 0; i < canonMiniPositions.Length; i++)
        {
            ShipMovementHandler.CanonType canonType = canons[i];
            if (canonType != ShipMovementHandler.CanonType.None)
            {
                canonMiniPositions[i].SetActive(true);
                canonMiniPositions[i].GetComponent<Image>().sprite = FindAnyObjectByType<CanonInfoPanel>().canonInfos[canonType].icon;
            }
            else
            {
                canonMiniPositions[i].SetActive(false);
            }
        }
    }
    
    public void UpdateCanonIconPositions(List<ShipMovementHandler.CanonType> canons)
    {
        for (int i = 0; i < canons.Count; i++)
        {
            canonPositionIconsImageComp[i].sprite = CanonIcons[(ShipMovementHandler.CanonType)canons[i]];
        }
    }
    
    private void CheckIfBuyButtonShouldBeInteractable()
    {
        if (XPHandler.instance.currentCanonPoints <= 0)
            return;
        if (selectedCanonPositionIndex == -1)
            return;
        if (selectedCanonTypeIndex == -1)
            return;

        canonUpgradeBuyButton.SetInteractableState(true);
    }
}
