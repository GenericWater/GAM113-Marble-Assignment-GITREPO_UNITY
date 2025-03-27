using UnityEngine;

public class Pickup : MonoBehaviour
{
    // FIX ERROR with Diamond Pickups - they are counting as two for some reason...

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
            {
                isCollected = true; // prevents the double trigger
            gameObject.SetActive(false);
            }
    }

}
