using UnityEngine;

[RequireComponent(typeof(Light))]
public class BlinkingLight : MonoBehaviour
{

    private Light lightComponent;

    private void Awake()
    {
        lightComponent = GetComponent<Light>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lightComponent.color = Color.Lerp(Color.red, Color.black, Mathf.Abs(Mathf.Sin(Time.time)));
    }
}
