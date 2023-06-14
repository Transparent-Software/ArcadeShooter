using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    //Sprint Mechanics
    private float _pressTime = 0f;
    private float _pressTimeTolerance = 2f;
    private bool sprintCooldown = false;
    public float multiplier = 1f;
    private float sprintMove, jump;

    //Jump Mechanics
    public float jumpHeight = 2.0f;
    public float gravity = -9.8f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool onGround;

    //Character Movement Mechanics
    //public CharacterController controller;
    public Rigidbody playerRB;
    public float currentSpeed = 200f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;


    //Animation Mechanics
    private Animator animator;
    private int isWalkingParamHash;
    private int isWalkingBackParamHash;
    private int isLeftStrafeParamHash;
    private int isRightStrafeParamHash;
    private int isSprintingParamHash;

    public void onSprint(InputAction.CallbackContext context)
    {

        sprintMove = context.ReadValue<float>();

        if(!sprintCooldown)
        {
            multiplier = sprintMove > 0 ? 2.0f : 1f;
            animator.SetBool(isSprintingParamHash, true);
        }
        else
        {
            multiplier = 1f;
            sprintCooldown = true;
            animator.SetBool(isSprintingParamHash, false);
        }

    }

    public void onJump(InputAction.CallbackContext context)
    {
        jump = context.ReadValue<float>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingParamHash = Animator.StringToHash("isWalking");
        isSprintingParamHash = Animator.StringToHash("isSprinting");
        isWalkingBackParamHash = Animator.StringToHash("isWalkingBack");
        isLeftStrafeParamHash = Animator.StringToHash("isLeftStrafe");
        isRightStrafeParamHash = Animator.StringToHash("isRightStrafe");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;



    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(onGround && jump > 0f)
        {
            playerRB.AddForce(new Vector3(playerRB.velocity.x, 1f, playerRB.velocity.z), ForceMode.Impulse);
        }


        if (direction.magnitude >= 0.1f)
        {

            checkForSprint();

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (Input.GetKey(KeyCode.W))
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            playerRB.AddForce(moveDir.normalized * currentSpeed * multiplier * Time.deltaTime, ForceMode.VelocityChange);


            //controller.Move(moveDir.normalized * currentSpeed * multiplier * Time.deltaTime);
        }
        else
        {
            float targetAngle = cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

        }

        handleAnimation();
    }

    private void checkForSprint()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {

            var maxSprintorDash = Input.GetKey(KeyCode.A) ? 0.25f : Input.GetKey(KeyCode.D) ? 0.25f : _pressTimeTolerance;

            if (_pressTime < maxSprintorDash)
            {
                _pressTime += Time.deltaTime;
            }
            else
            {
                sprintCooldown = true;
                animator.SetBool(isSprintingParamHash, false);
                multiplier = 1f;
            }
        }
        else
        {
            animator.SetBool(isSprintingParamHash, false);
            if (_pressTime > 0f)
            {
                _pressTime -= Time.deltaTime * 3;
            }
            else
            {
                if (sprintCooldown)
                {
                    sprintCooldown = false;
                }
            }
        }
    }

    private void handleAnimation()
    {
        if (Input.GetKey("w"))
        {
            animator.SetBool(isWalkingParamHash, true);
            //playerRB.AddForce(new Vector3(0f, 0f, currentSpeed), ForceMode.VelocityChange);

        }
        else
        {
            animator.SetBool(isWalkingParamHash, false);
        }

        if (Input.GetKey("s"))
        {
            animator.SetBool(isWalkingBackParamHash, true);
        }
        else
        {
            animator.SetBool(isWalkingBackParamHash, false);
        }

        if (Input.GetKey("a"))
        {
            animator.SetBool(isLeftStrafeParamHash, true);
        }
        else
        {
            animator.SetBool(isLeftStrafeParamHash, false);
        }

        if (Input.GetKey("d"))
        {
            animator.SetBool(isRightStrafeParamHash, true);
        }
        else
        {
            animator.SetBool(isRightStrafeParamHash, false);
        }
    }

    void FixedUpdate()
    {
    }

}
