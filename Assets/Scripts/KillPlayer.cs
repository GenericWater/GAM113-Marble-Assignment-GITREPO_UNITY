using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    // Script will manage player respawn points

    public GameObject player; // set on game objects and the script itself to work
    public static Transform currentCheckpoint; // last checkpoint touched

    // Default spawn position if no checkpoint has been reached
    private Vector3 defaultSpawnPosition = new Vector3(-6.073f, 2.761f, 5.636f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger hit by: " + other.name); // Always logs trigger activity

        if (other.CompareTag("Player"))
        {
            Debug.Log("KillPlayerRound triggered!");
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        Debug.Log("Respawn Player() activated");
        if (currentCheckpoint != null)
        {
            if (currentCheckpoint.CompareTag("Checkpoint")) // checks Tag of GameObj to ensure it is checkpoint -
                                                            // incase different objects become Checkpoints
            {
                Debug.Log("Respawn Player at last touched Checkpoint: " + currentCheckpoint.position);
                
                player.transform.position = currentCheckpoint.position; // set transform of player to position of currentCheckpoint 
            }
            else
            {
                Debug.LogWarning("Stored checkpoint is not tagged properly. Respawning at default.");
                player.transform.position = defaultSpawnPosition; // respawn player at beginning if checkpoint is not tagged properly.
            }



        }
        else 
        {
            Debug.LogWarning("No Checkpoint - respawning at default start position (RespawnPlayer())");
            player.transform.position = defaultSpawnPosition; // respawn player at beginning if checkpoint is not tagged properly.
        }

        if (rb != null) 
        {
            //rb.velocity = Vector3.zero; // OBSOLETE WITH ERROR - stops movement of player
            rb.linearVelocity = Vector3.zero; // stops movement of player
            rb.angularVelocity = Vector3.zero; // stops spinning of player
        }

    }

}
