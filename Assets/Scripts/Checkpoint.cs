using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    //public TagHandle CheckpointTag;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Checkpoint OnTriggerEnter called!");
        if (other.CompareTag("Player"))
        {
            KillPlayer.currentCheckpoint = transform;
            Debug.Log("Checkpoint set to: " + transform.position);
        }
    }

}
