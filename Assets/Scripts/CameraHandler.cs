using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    #region VariableDeclaration
    public Camera camera;
    [Header("Camera Follow Settings")]
    public float followSpeed;
    public float maxFOV;
    public float FoVChangeSpeed;
    [Header("Camera Shake Settings")]
    public float minShakeDuration = 0.25f;
    public float maxShakeDuration = 0.5f;
    public float minShakeMagnitude = 0.25f;
    public float maxShakeMagnitude = 0.75f;
    private ShipMovementHandler player;
    private Rigidbody player_rb;
    private bool followPlayer = false;
    private Vector3 initialOffset;
    private float speedPercentage;
    private float targetFoV = 30f;
    private float initialFOV; 
    #endregion

    private void Awake()
    {
        initialFOV = camera.fieldOfView;
    }
    
    void Start()
    {
        player = FindAnyObjectByType<ShipMovementHandler>();
        player_rb = player.GetComponent<Rigidbody>();
        ShipMovementHandler.OnDamaged += DoShake;
        initialOffset = transform.position - player.transform.position;
        followPlayer = true;
    }
    
    private void OnDisable()
    {
        ShipMovementHandler.OnDamaged -= DoShake;
    }

    void LateUpdate()
    {
        if (!followPlayer)
            return;

        transform.position = Vector3.Lerp(transform.position, player.transform.position + initialOffset, followSpeed * Time.deltaTime);
        speedPercentage = Mathf.Clamp01(player_rb.linearVelocity.magnitude / 15f);
        targetFoV = Mathf.Lerp(initialFOV, maxFOV, speedPercentage);
        camera.fieldOfView = Mathf.MoveTowards(camera.fieldOfView, targetFoV,FoVChangeSpeed * Time.deltaTime);
    }
    
    public void DoShake(float damage)
    {
        float shakeDuration = Mathf.Clamp(damage / 10f, minShakeDuration, maxShakeDuration);
        float shakeMagnitude = Mathf.Clamp(damage / 10f, minShakeMagnitude, maxShakeMagnitude);
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }
    
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = Vector3.zero; // Local origin relative to parent
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camera.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        camera.transform.localPosition = originalPos;
    }

    internal void FocusOnPlayer()
    {
        followPlayer = false;

    }

}
