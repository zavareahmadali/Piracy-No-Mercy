using UnityEngine;
using UnityEngine.UI;
public class CanonUpgradeBuyButton : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        SetInteractableState(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInteractableState(bool isActive)
    {
        button.interactable = isActive;
    }

}
