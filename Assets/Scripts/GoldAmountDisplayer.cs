using UnityEngine;
using TMPro;
using System.Threading;

public class GoldAmountDisplayer : MonoBehaviour
{
    public TMP_Text goldAmountText;
    public int increaseStep = 5;
    public float lerpSpeed = 0.1f;
    private int nextGoldAmount;
    private int currentGoldAmount;
    private float timer;

    void Start()
    {
        PlayerLootHandler.OnGoldAdded += HandleGoldAdded;
        timer = 0;
    }

    private void OnDisable()
    {
        PlayerLootHandler.OnGoldAdded -= HandleGoldAdded;
    }

    private void HandleGoldAdded(int amount)
    {
        // Update the UI or display the gold amount as needed
        nextGoldAmount = currentGoldAmount + amount;
    }

    void Update()
    {
        if (currentGoldAmount == nextGoldAmount)
            return;
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        currentGoldAmount += increaseStep;
        goldAmountText.text = currentGoldAmount.ToString("F0");
        timer = lerpSpeed;

    }
}
