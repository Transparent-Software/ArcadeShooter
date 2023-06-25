using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

public class PlayerController : MonoBehaviour
{


    //Sprint Mechanics
    private float _pressTime = 0f;
    private float _pressTimeTolerance = 2f;
    private bool sprintCooldown = false;
    public float multiplier = 1f;
    private float sprintMove, jump;

    //Jump Mechanics
    public float jumpHeight = 8.0f;
    public float gravity = -9.8f;
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    bool onGround;

    //Character Movement Mechanics
    public Rigidbody playerRB;
    public float currentSpeed = 200f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;
    public CinemachineVirtualCamera virtualCamera;

    //Aim Pitch Mechanics
    public GameObject spineRotation;
    public Vector3 rot;


    //Animation Mechanics
    private Animator animator;
    private int isWalkingParamHash;
    private int isWalkingBackParamHash;
    private int isLeftStrafeParamHash;
    private int isRightStrafeParamHash;
    private int isSprintingParamHash;
    private int isJumpingParamHash;
    private int isAimingIdleParamHash;
    private int isAimingRunningParamHash;
    private int isAimingWalkingParamHash;
    private int isInAirParamHash;


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
        Debug.Log(jump);
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
        isJumpingParamHash = Animator.StringToHash("isJumping");
        isAimingIdleParamHash = Animator.StringToHash("isAimingIdle");
        isAimingRunningParamHash = Animator.StringToHash("isAimingRunning");
        isAimingWalkingParamHash = Animator.StringToHash("isAimingWalking");
        isInAirParamHash = Animator.StringToHash("isInAir");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;



    }

    // Update is called once per frame
    void Update()
    {

        //"HOLD BREATH" Mechanic. Gets virtual camera sway and sets it to 0 if left ctrl is pressed while aiming

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1))
        {
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
        }
        else
        {
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.2f;
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.2f;
        }

        handleAnimation();
    }

    private void move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (direction.magnitude >= 0.1f)
        {

            checkForSprint();

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float moveAngle = targetAngle;

            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W))
            {
                targetAngle += 95f;
            }

            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
            {
                targetAngle -= 95f;
            }

            if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                targetAngle += 180f;
            }

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, moveAngle, 0f) * Vector3.forward;
           
            playerRB.AddForce(moveDir * currentSpeed * multiplier * Time.deltaTime, ForceMode.VelocityChange);

        }
        else
        {
            float targetAngle = cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);


        }

        if(onGround && playerRB.velocity.y > 0f)
        {
            Debug.Log("On slope and onGround");
            playerRB.AddForce(Vector3.down*1000f*Time.deltaTime, ForceMode.Acceleration);
        }
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
        if (!onGround)
        {
            animator.SetBool(isInAirParamHash, true);
        }
        else
        {
            animator.SetBool(isInAirParamHash, false);
        }

        if (Input.GetKey("w") && onGround)
        {
            animator.SetBool(isWalkingParamHash, true);
        }
        else
        {
            animator.SetBool(isWalkingParamHash, false);
        }

        if (Input.GetKey("s") && onGround)
        {
            animator.SetBool(isWalkingBackParamHash, true);
        }
        else
        {
            animator.SetBool(isWalkingBackParamHash, false);
        }

        if (Input.GetKey("a") && onGround)
        {
            animator.SetBool(isLeftStrafeParamHash, true);
        }
        else
        {
            animator.SetBool(isLeftStrafeParamHash, false);
        }

        if (Input.GetKey("d") && onGround)
        {
            animator.SetBool(isRightStrafeParamHash, true);
        }
        else
        {
            animator.SetBool(isRightStrafeParamHash, false);
        }


        if (Input.GetMouseButton(1) && onGround)
        {
            animator.SetBool(isAimingIdleParamHash, true);
        }
        else
        {
            animator.SetBool(isAimingIdleParamHash, false);
        }

        if (Input.GetMouseButton(1) && animator.GetBool(isSprintingParamHash) && onGround)
        {
            animator.SetBool(isAimingRunningParamHash, true);
        }
        else
        {
            animator.SetBool(isAimingRunningParamHash, false);
        }

        if (Input.GetMouseButton(1) && animator.GetBool(isWalkingParamHash) && onGround)
        {
            animator.SetBool(isAimingWalkingParamHash, true);

        }
        else
        {
            animator.SetBool(isAimingWalkingParamHash, false);
        }
    }

    void FixedUpdate()
    {
        move();

        if (onGround && Input.GetKey(KeyCode.Space))
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, jumpHeight, playerRB.velocity.z);
            animator.SetBool(isJumpingParamHash, true);
            animator.SetBool(isInAirParamHash, true);

        }
        else
        {
            animator.SetBool(isJumpingParamHash, false);
        }
    }

    void LateUpdate()
    {

        Transform joint = spineRotation.transform.Find("PT_Spine2");
        rot = new Vector3(rot.x, rot.y, cam.eulerAngles.x);

        if (Input.GetMouseButton(1))
        {
            //rot = new Vector3(rot.x, rot.y+25f, cam.eulerAngles.x);
            rot = new Vector3(rot.x, -cam.eulerAngles.x-0.05f, cam.eulerAngles.x);
        }

        joint.Rotate(rot, Space.Self);

        Transform jointFold = spineRotation.transform.Find("PT_Spine2");

        rot = new Vector3(rot.x, rot.y, rot.z - 100f);

        if (Input.GetKey(KeyCode.LeftAlt))
        {

            jointFold.Rotate(rot, Space.Self);

        }
        else
        {
            rot = Vector3.zero;
        }



    }

}
