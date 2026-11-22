using System;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnchorHandler : MonoBehaviour
{
    public GameObject aim;
    public float hookDistance = 10f;
    public GameObject ValidRegionIndicator;
    public LineRenderer lineRenderer;
    public LayerMask waterLayer;
    public float boostAngleTolerance = 10f;
    public float validHookAngle;
    [SerializeField] private InputActionReference rightClickActionRef;

    private HingeJoint joint;
    private Rigidbody rb;
    private bool isHooked = false;
    private bool isValidHook = false;
    private Vector3 startDirection;
    private Vector3 endDirection;
    public static Action OnPerfectHook;
    private Vector3 tempPos;
    private Ray tempRay;
    private GameObject tempAim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tempAim = Instantiate(aim, transform.position, aim.transform.rotation);
        //tempAim.transform.parent = this.transform;
        tempAim.SetActive(false);
        isHooked = false;
        isValidHook = false;
    }

    void FixedUpdate()
    {
        if (!isHooked)
        {
            tempPos = Mouse.current.position.ReadValue();

            tempRay = Camera.main.ScreenPointToRay(tempPos);
            if (Physics.Raycast(tempRay, out RaycastHit hit, Mathf.Infinity, waterLayer))
            {
                if (CanHookAtPosition(hit.point))
                {
                    if (!tempAim.activeInHierarchy)
                    {
                        tempAim.SetActive(true);
                        lineRenderer.gameObject.SetActive(true);
                        ValidRegionIndicator.SetActive(true);
                    }
                    tempAim.transform.position = hit.point;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);
                    isValidHook = true;
                }
                else
                {
                    if (tempAim.activeInHierarchy)
                    {
                        tempAim.SetActive(false);
                        lineRenderer.gameObject.SetActive(false);
                        ValidRegionIndicator.SetActive(false);
                    }
                    isValidHook = false;
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, tempAim.transform.position);
        }
    }
    private void OnEnable()
    {
        var rightClickAction = rightClickActionRef.action;
        rightClickAction.Enable();

        // Bind C# methods to press and release phases
        rightClickAction.started += OnRightClickPressed;
        rightClickAction.canceled += OnRightClickReleased;
    }
    private void OnDisable()
    {
        var rightClickAction = rightClickActionRef.action;

        // Unbind methods to prevent dangling memory pointers
        rightClickAction.started -= OnRightClickPressed;
        rightClickAction.canceled -= OnRightClickReleased;

        rightClickAction.Disable();

    }

    private void OnRightClickPressed(InputAction.CallbackContext context)
    {
        if (!isValidHook)
            return;

        // Triggers instantly on Key Down
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        ValidRegionIndicator.SetActive(false);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, waterLayer))
        {
            isHooked = true;
            Vector3 currentVelocity = rb.linearVelocity;
            Vector3 currentAngularVelocity = rb.angularVelocity;

            Vector3 worldPosition = hit.point;

            tempAim.transform.eulerAngles = new Vector3(tempAim.transform.eulerAngles.x, 0, 0);

            startDirection = transform.forward;

            joint = gameObject.AddComponent<HingeJoint>();
            joint.enablePreprocessing = false;
            joint.axis = Vector3.up;

            joint.connectedBody = tempAim.GetComponent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = new Vector3(Mathf.Sign(Vector3.Cross(transform.forward, tempAim.transform.position - transform.position).y) * (transform.position - tempAim.transform.position).magnitude, 0, 0);
            joint.connectedAnchor = Vector3.zero;

            rb.linearVelocity = currentVelocity;
            rb.angularVelocity = currentAngularVelocity;

        }
    }

    private void OnRightClickReleased(InputAction.CallbackContext context)
    {
        if (joint == null)
            return;
        endDirection = transform.forward;

        float angle = Vector3.Angle(startDirection, endDirection);
        if (Mathf.Abs(angle - 90) <= boostAngleTolerance || Mathf.Abs(angle - 180) <= boostAngleTolerance)
        {
            OnPerfectHook?.Invoke();
        }
        isHooked = false;
        joint.connectedBody = null;
        joint.anchor = Vector3.zero;
        Destroy(joint);
    }

    private bool CanHookAtPosition(Vector3 hookPoint)
    {
        if (Vector3.Distance(transform.position, hookPoint) > hookDistance)
            return false;
        if (Vector3.Dot(transform.forward, (transform.position - hookPoint).normalized) > Mathf.Sin(validHookAngle * Mathf.Deg2Rad))
            return false;
        if (Vector3.Dot(transform.forward, (transform.position - hookPoint).normalized) < Mathf.Sin(-validHookAngle * Mathf.Deg2Rad))
            return false;
        return true;
    }

}
