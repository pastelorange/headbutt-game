using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    [SerializeField]
    GameObject otherPlayer;

    [SerializeField]
    bool isTestDummy;

    [SerializeField]
    Collider headbutt, head, body;

    [SerializeField]
    GameObject healthBar;

    bool isKnockedOut;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockedOut) return;

        // Make sure the player is facing the otherPlayer
        if (otherPlayer.transform.position.x < transform.position.x)
        {
            transform.forward = new Vector3(-1, 0, 0);
        }
        else
        {
            transform.forward = new Vector3(1, 0, 0);
        }

        if (isTestDummy) return;

        // Relative movement controls using A and D keys. These forward and back controls are relative to the direction the player is facing.
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(0.01f, 0, 0);
            animator.SetBool("StepForward", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(-0.01f, 0, 0);
            animator.SetBool("StepBackward", true);
        }
        else
        {
            animator.SetBool("StepForward", false);
            animator.SetBool("StepBackward", false);
        }


        // Headbutt attack
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("Headbutt", true);
        }
    }

    // Runs if the headbutt successfully hits the other player
    // This animation event is roughly halfway through the headbutt animation
    public void OnHeadbuttHit()
    {
        // Check if headbutt collider hit the "Head" hitbox of the other player and the other player is not already in the "HeadHit" animation
        if (headbutt.bounds.Intersects(otherPlayer.GetComponent<PlayerController>().head.bounds) && !otherPlayer.GetComponent<Animator>().GetBool("HeadHit"))
        {
            // Stop the animation on this player
            animator.SetBool("Headbutt", false);

            // Reduce the other player's health nested in the healthBar object
            otherPlayer.GetComponent<PlayerController>().healthBar.GetComponent<HealthBar>().health -= 100;

            // If that blow was the final blow, then knockout the other player
            if (otherPlayer.GetComponent<PlayerController>().healthBar.GetComponent<HealthBar>().health <= 0)
            {
                otherPlayer.GetComponent<PlayerController>().RagdollKnockout();
            }

            // Play the hit animation on the other player
            otherPlayer.GetComponent<Animator>().SetBool("HeadHit", true);

            // Knockback the other player (x-coord) depending on the direction they are facing
            float knockbackDistance = 1;
            if (otherPlayer.transform.forward.x > 0)
            {
                otherPlayer.transform.position += new Vector3(-knockbackDistance, 0, 0);
            }
            else
            {
                otherPlayer.transform.position += new Vector3(knockbackDistance, 0, 0);
            }


        }
    }

    // Runs at the end of the headbutt animation
    public void ExitPlayerHitByHeadbutt()
    {
        animator.SetBool("HeadHit", false);
    }

    // Runs at the end of the headbutt animation
    // AKA if the attack whiffs
    public void OnHeadbuttEnd()
    {
        animator.SetBool("Headbutt", false);
    }

    void RagdollKnockout()
    {
        Debug.Log("Knockout!");

        isKnockedOut = true;

        // Disable animations
        animator.enabled = false;

        // Get all the rigidbodies
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        // Enable all the rigidbodies
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Apply a force relative to the direction the player is facing
        if (transform.forward.x > 0)
        {
            rigidbodies[0].AddForce(new Vector3(-40, 5, 0), ForceMode.Impulse);
        }
        else
        {
            rigidbodies[0].AddForce(new Vector3(40, 5, 0), ForceMode.Impulse);
        }
    }
}
