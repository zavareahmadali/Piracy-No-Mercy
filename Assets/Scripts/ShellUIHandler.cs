using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShellUIHandler : MonoBehaviour
{
    public TMPro.TMP_Text canonAmountText;
    public Image reloadBar;
    private Image canonTypeImage;
    [SerializeField]
    public Dictionary<ShipMovementHandler.CanonType, Sprite> canonTypeSprites;
    private ShipMovementHandler.CanonType canonType;
    private int canonAmount;
    private float currentReloadTime;

    private void Awake()
    {
        canonTypeImage = GetComponent<Image>();
    }

    void Start()
    {
        ShipMovementHandler.OnShellTypeChanged += UpdateCanonTypeUI;
        PlayerLootHandler.OnUpdateShells += UpdateCanonAmountUI;
        ShipCanonHandler.OnCanonReloadStarted += StartCanonReloadAnimation;

        UpdateCanonAmountUI(ShipMovementHandler.currentCanonType, PlayerLootHandler.GetShellCount(ShipMovementHandler.currentCanonType));
    }
    private void OnDisable()
    {
        ShipMovementHandler.OnShellTypeChanged -= UpdateCanonTypeUI;
        PlayerLootHandler.OnUpdateShells -= UpdateCanonAmountUI;
        ShipCanonHandler.OnCanonReloadStarted -= StartCanonReloadAnimation;
    }

    private void UpdateCanonAmountUI(ShipMovementHandler.CanonType t, int amount)
    {
        canonAmount = amount;
        canonAmountText.text = canonAmount.ToString();
    }

    private void UpdateCanonTypeUI(ShipMovementHandler.CanonType newCanonType)
    {
        canonType = newCanonType;
        canonTypeImage.sprite = canonTypeSprites[canonType];
        canonAmountText.text = PlayerLootHandler.GetShellCount(newCanonType).ToString();
    }
    void StartCanonReloadAnimation(float time)
    {
        reloadBar.fillAmount = 0;
        currentReloadTime = time;
        StartCoroutine(nameof(DoReloadBarAnimation));
    }
    IEnumerator DoReloadBarAnimation()
    {
        float timer = currentReloadTime;
        while (timer > 0)
        {
            reloadBar.fillAmount = (1 - (timer / currentReloadTime));
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        reloadBar.fillAmount = 1;
        yield return null;
    }
}
