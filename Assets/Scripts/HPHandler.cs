using System;
using UnityEngine;
using UnityEngine.UI;
public class HPHandler : MonoBehaviour
{
    public float MaxHP = 100f;
    public Image HPBar;
    public GameObject[] DamageIndicatorParticles;
    public DamageIndicator DamageIndicator;
    public float CurrentHP { get; private set; }
    public Action OnDie;
    private bool isDead = false;
    private float hpratio;
    public Action<float> OnHPChanged;
    private AIShipBase aiShipHandler;
    void Start()
    {
        isDead = false;
        CurrentHP = MaxHP;
        hpratio = CurrentHP / MaxHP;
        aiShipHandler = transform.GetComponent<AIShipBase>();
        aiShipHandler.TakeDamageAction += TakeDamage;
        //XPHandler.OnLevelUpdatedInfo += OnLevelUpdatedInfo;
        UpdateHPBar();
    }
    private void UpdateHPBar()
    {
        hpratio = (float)CurrentHP / MaxHP;
        if (aiShipHandler.GetType() == typeof(AIShipBig))
        {
            OnHPChanged?.Invoke(hpratio);
        }
        for (int i = 0; i < DamageIndicatorParticles.Length; i++)
        {
            if (DamageIndicatorParticles[i].gameObject.activeInHierarchy)
                continue;
            DamageIndicatorParticles[i].gameObject.SetActive((1 - hpratio) > (float)((i + 1) / (float)(DamageIndicatorParticles.Length + 1)));
        }

        if (HPBar == null)
            return;

        HPBar.fillAmount = CurrentHP / MaxHP;
        
    }

    internal void TakeDamage(bool isplayers, float damage)
    {
        if (isDead)
            return;

        CurrentHP -= damage;
        Instantiate(DamageIndicator, transform.position, Quaternion.identity).Show(damage.ToString("F2"));
        if (isplayers)
        {
            XPHandler.GetXP(XPConstants.XP_PER_ENEMY_DAMAGED);
        }
        if (CurrentHP < 0) CurrentHP = 0;
        UpdateHPBar();
        if (CurrentHP == 0)
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("Sink");
            if (HPBar != null)
            {
                HPBar.transform.parent.gameObject.SetActive(false); 
            }
            XPHandler.GetXP(XPConstants.XP_PER_ENEMY_KILLED);
            foreach (var di in DamageIndicatorParticles)
            {
                di.gameObject.SetActive(false);
            }
            OnDie?.Invoke();
        }
    }

    private void ChangeMaxHP(float newmaxHP)
    {
        MaxHP = newmaxHP;
        CurrentHP = MaxHP;
        UpdateHPBar() ;
    }
    private void OnLevelUpdatedInfo(XPLevelData xplevelData)
    {
        ChangeMaxHP(MaxHP + xplevelData.additionalHP);
    }
}
