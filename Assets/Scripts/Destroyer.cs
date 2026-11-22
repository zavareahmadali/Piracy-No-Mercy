using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public GameObject DestroyEffect;
    public float timer;
    void Start()
    {
        Destroy(gameObject, timer);
    }
    private void OnDestroy()
    {
        if (DestroyEffect==null)
            return;
        Instantiate(DestroyEffect);
    }
}
