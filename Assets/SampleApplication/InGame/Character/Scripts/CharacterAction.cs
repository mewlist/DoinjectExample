using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private float jumpUpFactor = 0.5f;
    [SerializeField] private float jumpDownFactor = 1.5f;
    [SerializeField] private bool grounded;

    private float HVelocity { get; set; }
    private float VVelocity { get; set; }
    private Vector3 Direction { get; set; }
    private Camera Camera { get; set; }
    private InGameInput Input { get; set; }

    private Vector2 Axis { get; set; }
    private bool TriggerJump { get; set; }
    private bool PressingJump { get; set; }

    public bool InputEnabled { get; set; }

    public void Teleport(Vector3 targetPosition)
    {
        characterController.enabled = false;
        transform.position = targetPosition;
        HVelocity = 0f;
        VVelocity = 0f;
        characterController.enabled = true;
    }

    private void Awake()
    {
        Input = new InGameInput();
        Input.Enable();

        Camera = Camera.main;
    }

    private void FixedUpdate()
    {
        ProcessInput();
        Move();
        if (grounded)
        {
            VVelocity = 0f;
            if (TriggerJump) Jump();
        }
        else
        {
            Fall();
        }

        characterController.Move(HVelocity * Direction * Time.deltaTime +
                                 VVelocity * Vector3.up * Time.deltaTime +
                                 characterController.skinWidth * Vector3.down);
        grounded = characterController.isGrounded;
        characterController.transform.LookAt(transform.position + Direction);
    }

    private void ProcessInput()
    {
        var lastPressingJump = PressingJump;
        if (InputEnabled)
        {
            PressingJump = Input.Player.Jump.IsPressed();
            TriggerJump = PressingJump && !lastPressingJump;
            Axis = Input.Player.Move.ReadValue<Vector2>();
        }
        else
        {
            PressingJump = false;
            TriggerJump = false;
            Axis = Vector2.zero;
        }
    }

    private void Fall()
    {
        var g = gravity;
        if (VVelocity > 0f)
        {
            if (PressingJump)
            {
                g *= jumpUpFactor;
            }
        }
        else
        {
            g *= jumpDownFactor;
        }
        VVelocity -= g * Time.deltaTime;
    }

    private void Jump()
    {
        VVelocity = jumpPower;
    }

    private void Move()
    {
        var cameraTransform = Camera.transform;
        var cameraForward = cameraTransform.forward;
        cameraForward = new Vector3(cameraForward.x, 0f, cameraForward.z);
        if (cameraForward.magnitude < 0.5f)
            cameraForward = new Vector3(cameraForward.y, 0f, cameraForward.z);

        cameraForward.Normalize();
        var targetDirection
            = Quaternion.Euler(90f, 0, 0) *
              Quaternion.LookRotation(cameraForward) *
              Axis;
        var targetVelocity = targetDirection.magnitude * speed;

        if (targetVelocity > 0f)
        {
            var flipped = Vector3.Dot(Direction, targetDirection) < -0.95f;
            var rotFactor = Mathf.Clamp(1f / HVelocity, 10f, 50f);
            Direction = flipped
                ? targetDirection
                : Vector3.Slerp(Direction, targetDirection, Time.deltaTime * rotFactor);
        }

        HVelocity = HVelocity < targetVelocity
            ? Mathf.Lerp(HVelocity, targetVelocity, Time.deltaTime * 10f)
            : targetVelocity;

    }
}