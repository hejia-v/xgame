using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour, IInputListener
{

    public float walkSpeed = 2;
    public float runSpeed = 6;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    Animator animator;
    Transform cameraT;

    bool needStopMove = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;

        InputManager.addListener(this);
    }

    void Update()
    {

    }

    public void UpdateTransform(out float curSpeed)
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        UpdateTransform(input, out curSpeed);
    }

    public void UpdateTransform(Vector2 input, out float curSpeed)
    {
        //return;
        Vector2 inputDir = input.normalized;

        Debug.Log(input);

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        float animationSpeedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        EventManager.sendEvent(EventType.PlayerMove, null);
        curSpeed = currentSpeed;
    }

    public void OnJoystickEvent(Vector2 deltaPos, float degrees)
    {
        float curSpeed = 0;
        UpdateTransform(deltaPos, out curSpeed);

        //needStopMove在update里处理
    }

    public bool OnInputEvent()
    {
        bool isPressMoveKey = false;
        bool isPressMoveUp = false;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            isPressMoveKey = true;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isPressMoveKey = true;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            isPressMoveKey = true;
            isPressMoveUp = true;
            needStopMove = true;
        }
        float curSpeed = 0;
        if (isPressMoveKey)
        {
            UpdateTransform(out curSpeed);
        }
        if (!isPressMoveKey && needStopMove)
        {
            UpdateTransform(out curSpeed);
            //Debug.Log("-----------" + curSpeed);
            if (Math.Abs(curSpeed) <= 1.401298E-44)
            {
                needStopMove = false;
            }
        }

        return false;
    }
    

}
