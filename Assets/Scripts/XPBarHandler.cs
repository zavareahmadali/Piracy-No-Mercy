using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class XPBarHandler : MonoBehaviour
{
    public Image xpBar;
    public TMP_Text xpLevelText;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        XPHandler.OnXPUpdated += UpdateXPBar;
        XPHandler.OnLevelUpdated += UpdateXPLevel;
    }

    private void UpdateXPBar(int gainedXP, int currentXP, int previousLevelXP, int nextLevelXP, int level)
    {
        if (xpBar != null)
        {
            //Debug.Log("gainedxp " + gainedXP + " currentxp " + currentXP + " previouslevelxp " + previousLevelXP + " nextlevelxp " + nextLevelXP + " level " + level); 
            xpBar.fillAmount = (float)(currentXP - previousLevelXP) / (nextLevelXP - previousLevelXP);
        }
    }

    private void UpdateXPLevel(int level)
    {
        if (xpLevelText != null)
        {
            animator.SetTrigger("LevelUp"); // Trigger the level-up animation
            xpLevelText.text = $"{level + 1}";// Display the current level
        }
    }

}
