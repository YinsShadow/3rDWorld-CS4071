using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine; //Camera package
public class AimStateManager : MonoBehaviour
{
    // Player States
    AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();
    // Camera
    [SerializeField] float mouseSense = 1;
    float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    // More Camera
    [HideInInspector] public Animator anim;
    [HideInInspector] public CinemachineCamera vCam;
    public float adsFov = 40;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vCam = GetComponentInChildren<CinemachineCamera>();
        hipFov = vCam.Lens.FieldOfView;
        anim = GetComponentInChildren<Animator>();
        SwitchState(Hip);
    } 

    // Update is called once per frame
    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis += Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -80, 80);

        vCam.Lens.FieldOfView = Mathf.Lerp(vCam.Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);// FOV change on aim state change

        currentState.UpdateState(this); // Update the animation state
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
