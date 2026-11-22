using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIShipCanonHandler : BaseCanonHandler
{
    public GameObject canonPrefab;

    public override void OnShoot(InputAction.CallbackContext ctx)
    {
        base.OnShoot(ctx);
        GameObject canon = Instantiate(canonPrefab, shootPoint.position, Quaternion.identity);
        canon.GetComponent<Rigidbody>().AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        canon.GetComponent<CanonBallHandler>().Initialize(transform.root.gameObject, 0);
        
    }
}

