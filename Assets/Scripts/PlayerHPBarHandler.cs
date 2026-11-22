using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBarHandler : MonoBehaviour
{
    public Image Background;
    public Image actualHPBar;
    private Image hpbar;
    
    private void Awake()
    {
        hpbar = GetComponent<Image>();
    }
    void Start()
    {
        PlayerHPHandler.UpdateHP += UpdateHPBar;
        PlayerHPHandler.MaxHPUpdated += UpdateMaxHP;
    }

    private void OnDisable()
    {
        PlayerHPHandler.UpdateHP -= UpdateHPBar;
        PlayerHPHandler.MaxHPUpdated -= UpdateMaxHP;

    }

    void UpdateHPBar(float currentHP, float maxHP)
    {
        if (hpbar == null)
            return;
        hpbar.fillAmount = currentHP / maxHP;
    }

    void UpdateMaxHP(float newMaxHP, float oldMaxHP)
    {
        Background.rectTransform.sizeDelta = new Vector2((1 + (newMaxHP / (oldMaxHP * 10))) * Background.rectTransform.sizeDelta.x, Background.rectTransform.sizeDelta.y);
        hpbar.rectTransform.sizeDelta = new Vector2(((1 + (newMaxHP / (oldMaxHP * 10))) * 1.1f) * hpbar.rectTransform.sizeDelta.x, hpbar.rectTransform.sizeDelta.y);
        actualHPBar.rectTransform.sizeDelta = new Vector2(((1 + (newMaxHP / (oldMaxHP * 10))) * 1.1f) * actualHPBar.rectTransform.sizeDelta.x, actualHPBar.rectTransform.sizeDelta.y);

    }
}
