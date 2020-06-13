using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    /// <summary>
    /// Script Compilado em C# do novo input system do unity, o antigo e default nao ria funcionar
    /// </summary>
    private Controls _Controls;


    private Vector2 _LookAxis;
    /// <summary>
    /// Atual angulo de Visao, X para horizontal, y para vertical.
    /// Para passar para Vector3 fazer(x,0,y).
    /// </summary>
    public Vector2 LookAxis => _LookAxis;

    /// <summary>
    /// Distancia maxima da camera ate ao centro
    /// </summary>
    [Range(2f, 10f)]
    public float Maxdistance_from_center = 3.5f;

    /// <summary>
    /// Maxima altura da camera
    /// </summary>
    [Range(1f, 10f)]
    public float Maxheight = 2f;
    /// <summary>
    /// Minimo de altura da camera
    /// </summary>
    [Range(0f, 3f)]
    public float Minheight = 1;
    /// <summary>
    /// Distancia minima da camera ate ao centro
    /// </summary>
    [Range(2f, 5f)]
    public float Mindistance_from_center = 2.5f;
    /// <summary>
    /// Angulo maximo da camera em relacao ao centro
    /// </summary>
    [Range(27f, 40f)]
    public float UpperLimit = 35f;

    /// <summary>
    /// Angulo minimo da camera em ralacao ao centro
    /// </summary>
    [Range(1f, 15f)]
    public float LowerLimit = 10f;

    /// <summary>
    /// O jogador principal
    /// </summary>
    public PlayerMovement center;

    /// <summary>
    /// Velocidade da rotacao do jogador
    /// </summary>
    [Range(0.0005f, 1f)]
    public float lookSpeed = 0.005f;
    private bool CoroutineRunning = false;

    /// <summary>
    /// Corrido sempre que SetActiver passa para true
    /// </summary>
    private void OnEnable()
    {
        _Controls = new Controls();
        _Controls.Player.MouseMovement.performed += HandleLook;
        _Controls.Player.MouseMovement.Enable();
        _Controls.Player.DirectionalMovement.performed += HandleMove;
        _Controls.Player.DirectionalMovement.Enable();

    }

    private void HandleMove(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<Vector2>() != Vector2.zero && !CoroutineRunning)
        {
            StartCoroutine(CenterCamToPlayer());
        }
    }

    private IEnumerator CenterCamToPlayer()
    {
        CoroutineRunning = true;
        if (Vector3.Distance(transform.position, center.transform.position) > Maxdistance_from_center)
        {
            //Debug.Log(Vector3.Distance(transform.position, center.transform.position));
            // transform.position = Vector3.Lerp(transform.position, center.transform.position, 0.9f * Time.deltaTime);
            // transform.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, center.transform.position, Vector3.Distance(transform.position, center.transform.position) - 3f), 0.2f * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, center.transform.position, Vector3.Distance(transform.position, center.transform.position)), 3f * Time.deltaTime);
            Quaternion look = Quaternion.LookRotation(center.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * 4);
        }
        else if (Vector3.Distance(transform.position, center.transform.position) < Mindistance_from_center)
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, center.transform.position, -10f), 2f * Time.deltaTime);
            Quaternion look = Quaternion.LookRotation(center.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * 4);
        }
        else if (Quaternion.LookRotation(center.transform.position - transform.position) != transform.rotation)
        {
            Quaternion look = Quaternion.LookRotation(center.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, look, Time.deltaTime * 4);
        }
        else
        {
            CoroutineRunning = false;
            StopCoroutine(CenterCamToPlayer());
        }
        yield return new WaitForSeconds(Time.deltaTime / 2);
        StartCoroutine(CenterCamToPlayer());

    }

    private void OnDisable()
    {
        _Controls.Player.MouseMovement.performed -= HandleLook;
        _Controls.Player.MouseMovement.Disable();
        _Controls.Player.DirectionalMovement.performed -= HandleMove;
        _Controls.Player.DirectionalMovement.Disable();
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
        Limit_angle();
    }


    // Start is called before the first frame update
    private void Start()
    {
        if (center == null)
        {
            center = FindObjectOfType<PlayerMovement>();
        }
        Maxheight += center.transform.position.y;
        Minheight += center.transform.position.y;
        transform.LookAt(center.transform);
    }

    private void Limit_angle()
    {

        if (transform.position.y > Maxheight)
        {
            transform.position = new Vector3(transform.position.x, Maxheight, transform.position.z);
        }
        else if (transform.position.y < Minheight)
        {
            transform.position = new Vector3(transform.position.x, Minheight, transform.position.z);
        }
    }

}
