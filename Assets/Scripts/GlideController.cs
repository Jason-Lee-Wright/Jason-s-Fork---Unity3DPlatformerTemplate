using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private GameObject StaminaBar;
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
        StaminaBar = GameObject.Find("Stamina Piviot");
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

        currentStamina = maxGlideTime;
    }

    /// <summary>
    /// Updates stamina depletion and regeneration.
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
        else if (currentStamina > maxGlideTime)
        {
            currentStamina = maxGlideTime;
        }

        if (isGliding)
        {
            ApplyGlide();
            currentStamina -= Time.deltaTime;
            if (currentStamina <= 0)
            {
                StopGlide();

                currentStamina = 0;
            }
        }
    }
}
