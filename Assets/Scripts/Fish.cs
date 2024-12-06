using UnityEngine;

public class Fish : MonoBehaviour
{
    public AudioSource chompingSound;

    void Start()
    {
        // Ensure the AudioSource component is attached
        chompingSound = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player fish collides with another fish
        if (collision.gameObject.tag == "Fish")
        {
            // Play the chomping sound
            chompingSound.Play();

            // Optionally, you can destroy the other fish
            Destroy(collision.gameObject);
        }
    }
}
