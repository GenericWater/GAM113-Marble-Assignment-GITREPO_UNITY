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

    private Rigidbody rb; // refrence to rigid body to access ONLY in this script
    private float movementX;
    private float movementY;

    private int count; // stores value of pickups / num of pickups

    private bool isGrounded; // checks to see if player is on ground

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
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
    void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 13) // if you change number of collectibles MUST UPDATE
        {
            winTextObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        // NEW Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
    }

    //detects contact between player and gameobject without causing a physical collision
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp")) // checks tag of gameObject
        {
            other.gameObject.SetActive(false); // disables gameObject on collision - sets pickups to false
            count = count + 1; // adds one to total count of pickups 

            SetCountText(); // updates UI
        }
    }

}
