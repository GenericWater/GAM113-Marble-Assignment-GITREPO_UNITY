using UnityEngine;
using UnityEngine.InputSystem; // allows access to the input system on Player (Component) in this script
using TMPro; // impots Text Mesh Pro element

public class PlayerController : MonoBehaviour
{
    public float speed = 0; // speed of player - can be editied in GUI
    public float jumpForce = 5f; // how high the player can jump NEW
    public Transform groundCheck; // transform to see if player is on ground NEW
    public float groundDistance = 0.4f; // how close the player is to the ground NEW
    public LayerMask groundMask;

    public TextMeshProUGUI countText; // holds a refrence to UI text component and the countText game componenet
    public GameObject winTextObject; // win text for game

    public GameObject killPlayer; // controls the checkpoints

    public float gamePickups = 1;

    private Rigidbody rb; // refrence to rigid body to access ONLY in this script
    private float movementX;
    private float movementY;

    private int count; // stores value of pickups / num of pickups

    private bool isGrounded; // checks to see if player is on ground

    private KillPlayer killPlayerScript; // call to KillPlayer Script to use in this script for Respawn

    // Added "Mario's" Coyote Time & Jump Buffering to fix jump lag during input
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    // FIX unlimited air jumps!
    private bool hasJumped = false;
    // added a check to only reset hasJumped when the player was not grounded in the previous frame and is grounded now.
    private bool wasGrounded;

    private float jumpCooldown = 0.25f; // MINIMUM time between jumps
    private float jumpCooldownCounter = 0f; // Timer for jumping

    public Vector3 groundCheckOffset = new Vector3(0f, -0.6f, 0f); // tweak Y for your player’s size

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);

        killPlayerScript = killPlayer.GetComponent<KillPlayer>();

        groundCheck.SetParent(null); // groundcheck is attached to prefab, but spins with player -> made parent NULL to stop spinning
    }

    void OnMove(InputValue movementValue)
    {
        // Function body

        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // NEW jump function

    public void OnJump()
    {
        Debug.Log("Jump pressed");
        jumpBufferCounter = jumpBufferTime; // store jump input briefly

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !hasJumped && jumpCooldownCounter <= 0f)
        {
            Debug.Log("JUMP TRIGGERED");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpBufferCounter = 0f;
            hasJumped = true;
            jumpCooldownCounter = jumpCooldown; // Start cooldown
        }

    }

    public void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= gamePickups) // changed to check public game number of pickups - set by editors in inspector not game designers.
        {
            winTextObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        jumpCooldownCounter -= Time.fixedDeltaTime;

        groundCheck.position = transform.position + groundCheckOffset; //ensures groundCheck stays directly under the player, without spinning, every frame

        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // UPDATE COYOTE TIME
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            hasJumped = false; // Reset jump lock when player lands
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
        // UPDATE JUMP BUFFER
        jumpBufferCounter -= Time.fixedDeltaTime;

        // PERFORM JUMP IF COUNTETRS ARE VALID
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !hasJumped)
        {
            Debug.Log("JUMP TRIGGERED");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpBufferCounter = 0f; // clears the jump input after player jumps
            hasJumped = true;
        }


        Vector3 movement = new Vector3(movementX, 0f, movementY).normalized;

        float controlMultiplier = isGrounded ? 1f : 0.4f; // Better air control

        // Add horizontal force
        rb.AddForce(movement * speed * controlMultiplier, ForceMode.Force);

        // UPDATE the previous grounded state
        wasGrounded = isGrounded;

        // Heavier falling for snappy landing (does not use velocity!)
        //if (!isGrounded)
        //{
            //rb.AddForce(Physics.gravity * 2f, ForceMode.Acceleration);
        //}

    }

    //detects contact between player and gameobject without causing a physical collision
    private void OnTriggerEnter(Collider other) // checks for collison with Pickup items
    {
        if(other.gameObject.CompareTag("PickUp")) // checks tag of gameObject
        {
            other.gameObject.SetActive(false); // disables gameObject on collision - sets pickups to false
            count++; // adds one to total count of pickups | count++; works the same

            SetCountText(); // updates UI
        }
    }

}
