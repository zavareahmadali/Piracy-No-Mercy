using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;

public class PlayerHPBarHandler : MonoBehaviour
{
    [SerializeField] private float smoothSpeed;
    public Image Background;
    public Image actualHPBar;
    private Image hpbar;
    private float targetHP;
    private void Awake()
    {
        hpbar = GetComponent<Image>();
        
    }
    private void Start()
    {
        PlayerHPHandler.UpdateHP += UpdateHPBar;
        PlayerHPHandler.MaxHPUpdated += UpdateMaxHP;
    }

    private void OnDisable()
    {
        PlayerHPHandler.UpdateHP -= UpdateHPBar;
        PlayerHPHandler.MaxHPUpdated -= UpdateMaxHP;

    }

    private void UpdateHPBar(float currentHP, float maxHP)
    {
        if (hpbar == null)
            return;
        targetHP = currentHP / maxHP;
        //Debug.Log(targetHP);
        hpbar.fillAmount = currentHP / maxHP;
    }

    private void UpdateMaxHP(float newMaxHP, float oldMaxHP)
    {
        Background.rectTransform.sizeDelta = new Vector2((1 + (newMaxHP / (oldMaxHP * 10))) * Background.rectTransform.sizeDelta.x, Background.rectTransform.sizeDelta.y);
        hpbar.rectTransform.sizeDelta = new Vector2(((1 + (newMaxHP / (oldMaxHP * 10))) * 1.1f) * hpbar.rectTransform.sizeDelta.x, hpbar.rectTransform.sizeDelta.y);
        actualHPBar.rectTransform.sizeDelta = new Vector2(((1 + (newMaxHP / (oldMaxHP * 10))) * 1.1f) * actualHPBar.rectTransform.sizeDelta.x, actualHPBar.rectTransform.sizeDelta.y);

    }

    private void FixedUpdate()
    {
        //if (hpbar == null)
        //    return;
        //if (hpbar.fillAmount != targetHP)
        //{
        //    targetHP -= smoothSpeed * Time.deltaTime;
        //    hpbar.fillAmount = targetHP;
        //}
    }
}
