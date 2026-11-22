using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public TMP_Text Number;

    public void Show(string text)
    {
        Number.text = text;
        Destroy(gameObject, 2f);
    }
}
