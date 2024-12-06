using Unity.Hierarchy;
using UnityEngine;

public class Player : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed;

    float horizontalInput, verticalInput;
    int score = 0;

    public GameObject winText;
    public int winScore;

    public AudioSource chompingSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chompingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        horizontalInput = joystick.Horizontal * moveSpeed;
        verticalInput = joystick.Vertical * moveSpeed;

        transform.Translate(horizontalInput, verticalInput, 0);
        if(horizontalInput < 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(horizontalInput > 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Contains ("Food"))
        {
            score++;

            chompingSound.Play();

            Destroy(collision.gameObject);

            if(score >= winScore)
            {
                winText.SetActive(true);
                
            }
        }
    }
}
