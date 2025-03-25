using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player; // refrenece player GameObject's position -> link in inspector what the player is
    private Vector3 offset; // set value in script 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        offset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void LateUpdate() // will update after unity runs their scripts that go first; ensuring this is the last update of the frame -> following the player
    {
        // set players trnasform position
        transform.position = player.transform.position + offset;
        
    }
}
