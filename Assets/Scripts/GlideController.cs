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
    [SerializeField]
    private bool isGliding;
    [SerializeField]
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
            StopGlide(); // Ensure stopping happens on button release
        }
    }

    /// <summary>
    /// Starts the gliding effect, reducing gravity.
    /// </summary>
    private void StartGlide()
    {
        Debug.Log("Start Gliding");
        isGliding = true;
    }
    private void ApplyGlide()
    {
        if (!isGliding) return; // Stop modifying velocity if we're not gliding

        // Smooth gliding effect
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * glideGravityScale, rb.velocity.z);
    }

    /// <summary>
    /// Stops gliding and restores normal gravity.
    /// </summary>
    private void StopGlide()
    {
        Debug.Log("Stop Gliding");
        isGliding = false;

        // Reset vertical velocity to prevent the player from staying in the air
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply normal gravity
        rb.useGravity = true;
    }

    /// <summary>
    /// Updates stamina depletion and regeneration.
    /// </summary>
    public override void Update()
    {
        base.Update();
        if (isGliding)
        {
            ApplyGlide();
            currentStamina -= Time.deltaTime;
            if (currentStamina <= 0)
            {
                StopGlide();
            }
        }

        // Stop gliding if we touch the ground
        if (isGrounded && isGliding)
        {
            Debug.Log("Stop Glideing Please");
            StopGlide();
        }

        // Regenerate stamina on the ground
        if (isGrounded)
        {
            Debug.Log("Stop Glideing No stamina");
            StopGlide();
            currentStamina = Mathf.Min(currentStamina + (staminaRegenRate * Time.deltaTime), maxGlideTime);
        }
    }
}
