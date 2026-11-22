using UnityEngine;
using UnityEngine.UI;
public class BossHPUI : MonoBehaviour
{
    public Image hpImage;
    private HPHandler bossShipHPHandler;
    public void OnEnable()
    {
        bossShipHPHandler = FindAnyObjectByType<AIShipBig>().GetComponent<HPHandler>();
        bossShipHPHandler.OnDie += Hide;
        GetComponent<Animator>().SetBool("Show", true);
        bossShipHPHandler.OnHPChanged += UpdateHPBar;
    }
    public void OnDisable()
    {
        if (bossShipHPHandler != null)
        {
            bossShipHPHandler.OnHPChanged -= UpdateHPBar;
            bossShipHPHandler.OnDie -= Hide;
        }
    }

    private void Hide()
    {
        GetComponent<Animator>().SetBool("Show", false);
    }
    public void UpdateHPBar(float ratio)
    {
        hpImage.fillAmount = ratio;
    }
}
