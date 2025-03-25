using UnityEngine;

public class Rotator : MonoBehaviour
{
    // will not be using forces to rotate pickup
    //needs to make pickup object spin while game is active

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(5f, 30f, 0f) * Time.deltaTime); // delta time is a float representing the difference in seconds
                                                                    // since the last frame update

    }
}
