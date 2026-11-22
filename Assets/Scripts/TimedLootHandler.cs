using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TimedLootHandler : LootHandler
{
    [SerializeField] float lootTime = 2f;
    [SerializeField] Image lootTimerImage;

    public override void LootAreaEntered()
    {
        StartCoroutine(nameof(StartCountDown));
    }

    public override void LootAreaExited()
    {
        StopCoroutine(nameof(StartCountDown));
        lootTimerImage.fillAmount = 0;
    }

    private IEnumerator StartCountDown()
    {
        float timer = lootTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            
            lootTimerImage.fillAmount = Mathf.Clamp01(timer / lootTime);

            yield return null;
        }
        animator.SetTrigger("Looted");
        GetLooted();
        yield return null;
    }

}
