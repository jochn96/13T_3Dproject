using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;  //이동속도
    public float jumpPower;  //점프력
    public float dashSpeed;  //대쉬속도
    public bool isDash;      //대쉬여부판단
    private Vector2 MovementInput;
    public LayerMask groundLayer;

    [Header("Look")]
    public Transform cameraSlot;
    public float minXLook;
    public float maxXLook;
    private float camXRotation;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody rb;
    //private PlayerCondition condition;
    private Coroutine coroutine;

    public GameObject howToPlay;
    private void Awake()
    {
        //condition = GetComponent<PlayerCondition>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    #region Move
    private void Move()
    {
        Vector3 diraction = transform.forward * MovementInput.y + transform.right * MovementInput.x;

        if (isDash)
            diraction *= dashSpeed;
        else
            diraction *= moveSpeed;

        diraction.y = rb.velocity.y;

        rb.velocity = diraction;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            MovementInput = Vector2.zero;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) //대쉬키를 누르면
        {
            isDash = true;
            StartAddCor(10);  //스테미너 소모량
        }
        else
            isDash = false;
    }
    private IEnumerator CoTimer(float useStamina)
    {
        while (0f < useStamina)
        {
            if (isDash)  //만약에 대쉬중이라면 스테미너를 스테미너 소모량만큼 초당 소모 
                //condition.UseStamina(useStamina * Time.deltaTime);

            yield return null;
        }
    }
    public void StartAddCor(float useStamina)
    {
        if (coroutine != null)  //Coroutine이 실행중이면 중단
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(CoTimer(useStamina));  //Corutine 실행
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))
                return true;
        }
        return false;
    }
    #endregion

    #region Look
    void CameraLook()
    {
        camXRotation += mouseDelta.y * lookSensitivity;
        camXRotation = Mathf.Clamp(camXRotation, minXLook, maxXLook);
        cameraSlot.localEulerAngles = new Vector3(-camXRotation, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    #endregion

    #region Inventory

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    #endregion
}
