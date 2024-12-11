using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    //Movement
    public float moveSpeed = 3f;
    [HideInInspector] public Vector3 dir;
    float hzInput, vInput;
    CharacterController controller;
    //GroundCheck
    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;
    //Gravity
    [SerializeField] float gravity = -9.81f; // True to IRL standards!
    Vector3 velocity;

    MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();

    [HideInInspector] public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);// Starting state set
    }

    // Update is called once per frame
    void Update()
    {
        GetDirectionAndMove(); //Check for movement and do so if required
        Gravity(); // Apply gravity

        anim.SetFloat("hzInput", hzInput);
        anim.SetFloat("vInput", vInput);

        currentState.UpdateState(this); // Update the animation state
    }

    public void SwitchState(MovementBaseState state) // Change anumation state
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void GetDirectionAndMove()// Player facing direction check and movement
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput; // Always move relative to facing direction

        controller.Move(dir.normalized * moveSpeed * Time.deltaTime); // Move the player
    }

    bool IsGrounder() // Is the player on the gorund?
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z); 
        if(Physics.CheckSphere(spherePos, controller.radius -0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounder()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y<0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime); // Apply gravity to player
    }

    private void OnDrawGizmos()
    {
        if (controller == null) 
        {
            controller = GetComponent<CharacterController>();
            if (controller == null)
            {
                return; // Exit if controller is still null
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f); // Draw a sphere for ground check
    }
}
