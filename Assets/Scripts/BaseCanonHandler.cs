using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseCanonHandler : MonoBehaviour
{
    public Transform shootPoint;
    public float shootForce = 1;
    public float recoilForce = 1;
    public GameObject muzzleFlashParticleSystem;

    public virtual void OnShoot(InputAction.CallbackContext ctx)
    {
        // This method is intended to be overridden in derived classes.
        if (GameManagerHandler.isGamePaused)
            return;

    }
}
