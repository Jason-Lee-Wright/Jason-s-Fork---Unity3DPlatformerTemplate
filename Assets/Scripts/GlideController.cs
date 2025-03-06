using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Extends AdvancedMoveController with a gliding system that consumes stamina.
/// Stamina depletes gradually while gliding and regenerates upon landing.
/// </summary>
public class GlideController : AdvancedMoveController
{
    [Header("Glide Settings")]
    [Tooltip("Maximum glide time in seconds before stamina depletes")]
    public float maxGlideTime = 5f;
    [Tooltip("Gravity scale while gliding (lower means slower descent)")]
    public float glideGravityScale = 0.5f;
    [Tooltip("Stamina regeneration rate per second after landing")]
    public float staminaRegenRate = 1.5f;

    [SerializeField]
    private float currentStamina;
    private bool isGliding;
    private PlayerInput playerInput;

    /// <summary>
    /// Initialize variables and setup input bindings.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        currentStamina = maxGlideTime;
    }

    /// <summary>
    /// Handles input for starting and stopping glide.
    /// </summary>
    public void OnGlide(InputValue value)
    {
        Debug.Log("Starting Glide code");
        if (value.isPressed && !isGrounded && currentStamina > 0)
        {
            StartGlide();
        }
        else
        {
            StopGlide();
        }
    }

    /// <summary>
    /// Starts the gliding effect, reducing gravity.
    /// </summary>
    private void StartGlide()
    {
        Debug.Log("Start Gliding");
        isGliding = true;
        rb.velocity = new Vector3(rb.velocity.x, glideGravityScale, rb.velocity.z);
    }

    /// <summary>
    /// Stops gliding and restores normal gravity.
    /// </summary>
    private void StopGlide()
    {
        Debug.Log("Stop Gliding");
        isGliding = false;
    }

    /// <summary>
    /// Updates stamina depletion and regeneration.
    /// </summary>
    public override void Update()
    {
        if (isGliding)
        {
            Debug.Log("Gliding");
            currentStamina -= Time.deltaTime;
            if (currentStamina <= 0)
            {
                StopGlide();
            }
        }
        else if (!isGliding)
        {
            Debug.Log("Regeneration");
            currentStamina = Mathf.Min(currentStamina + (staminaRegenRate * Time.deltaTime), maxGlideTime);
        }
    }
}
