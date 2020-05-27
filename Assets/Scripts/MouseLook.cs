using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private Controls _Controls;
    private Vector2 _LookAxis;
    public Vector2 LookAxis => _LookAxis;

    [Range(2f, 5f)]
    public float Maxdistance_from_center = 3.5f;

    [Range(2f, 5f)]
    public float Mindistance_from_center = 2.5f;
    [Range(27f, 40f)]
    public float UpperLimit = 35f;
    [Range(1f, 15f)]
    public float LowerLimit = 10f;
    public PlayerMovement center;

    [Range(0.0005f, 1f)]
    public float lookSpeed = 0.005f;
    private void OnEnable()
    {
        _Controls = new Controls();
        _Controls.Player.MouseMovement.performed += HandleLook;
        _Controls.Player.MouseMovement.Enable();
        PlayerMovement.current.Moveu += HandleMove;
    }

    private void HandleMove()
    {
        if (Vector3.Distance(transform.position, center.transform.position) > Maxdistance_from_center ||
            Vector3.Distance(transform.position, center.transform.position) < Mindistance_from_center)
        {
            Debug.Log(Vector3.Distance(transform.position, center.transform.position));
            // transform.position = Vector3.Lerp(transform.position, center.transform.position, 0.2f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, center.transform.position, Vector3.Distance(transform.position, center.transform.position) - 3f), 0.2f * Time.deltaTime);
            transform.LookAt(center.transform);
        }
    }

    private void OnDisable()
    {
        _Controls.Player.MouseMovement.performed -= HandleLook;
        _Controls.Player.MouseMovement.Disable();
    }
    private void HandleLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        try
        {
            _LookAxis = context.ReadValue<Vector2>();

            RotateCam();
        }
        catch
        {
            throw new System.NotImplementedException();
        }
    }

    private void RotateCam()
    {
        transform.RotateAround(center.transform.position, transform.TransformDirection(Vector3.up), LookAxis.x * lookSpeed);
        transform.RotateAround(center.transform.position, transform.TransformDirection(Vector3.right), LookAxis.y * lookSpeed);

        switch (transform.eulerAngles.x)
        {
            case float n when (n > UpperLimit):
                transform.RotateAround(center.transform.position, transform.TransformDirection(Vector3.right), UpperLimit - n);
                break;
            case float n when (n < LowerLimit):
                transform.RotateAround(center.transform.position, transform.TransformDirection(Vector3.right), LowerLimit - n);
                break;
        }


        float z = transform.eulerAngles.z;
        transform.Rotate(0, 0, -z);
    }


    // Start is called before the first frame update
    private void Start()
    {
        if (center == null)
        {
            center = FindObjectOfType<PlayerMovement>();
        }

        transform.LookAt(center.transform);
    }

}
