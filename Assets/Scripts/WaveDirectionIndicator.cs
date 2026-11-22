using UnityEngine;

public class WaveDirectionIndicator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform playerTransform;
    public float scrollSpeed;
    public float duration;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        Destroy(gameObject, duration);
    }

    public void Initialize(Vector3 startPosition, Transform playerTransform)
    {
        lineRenderer.SetPosition(0, startPosition);
        this.playerTransform = playerTransform;
    }

    private void FixedUpdate()
    {
        lineRenderer.SetPosition(1, playerTransform.position);
    }
}
