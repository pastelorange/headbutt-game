using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    GameObject otherPlayer;

    [SerializeField]
    bool isTestDummy;

    [SerializeField]
    Collider headCollider;

    [SerializeField]
    //GameObject healthBar;

    bool isKnockedOut;
    float crankedTimer = 10f;

    Rigidbody rb;
    bool isGrounded = false;

    float? lastGroundedTime;
    float? jumpButtonPressedTime;
    float jumpButtonGracePeriod = 0.2f;
    float ySpeed;
    bool isJumping = false;
    float jumpSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CrankedTimer());
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
            animator.SetBool("IsMoving", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(-0.01f, 0, 0);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        // Headbutt attack
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("Headbutt", true);
        }

        /*
        // Jumping code below
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            ySpeed = -0.5f;
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            animator.SetBool("IsFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            animator.SetBool("IsGrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("IsFalling", true);
            }
        }*/
    }

    // Runs if the headbutt successfully hits the other player
    // This animation event is roughly halfway through the headbutt animation
    public void OnHeadbuttHit()
    {
        // Check if headbutt collider intersected the CapsuleCollider of the other player and the other player is not already in the "HeadHit" animation
        if (headCollider.bounds.Intersects(otherPlayer.GetComponent<CapsuleCollider>().bounds) && !otherPlayer.GetComponent<Animator>().GetBool("HeadHit"))
        {
            // Stop the animation on this player
            animator.SetBool("Headbutt", false);

            // Reduce the other player's health nested in the healthBar object
            /*otherPlayer.GetComponent<PlayerController>().healthBar.GetComponent<HealthBar>().health -= 100;

            // If that blow was the final blow, then knockout the other player
            if (otherPlayer.GetComponent<PlayerController>().healthBar.GetComponent<HealthBar>().health <= 0)
            {
                otherPlayer.GetComponent<PlayerController>().RagdollKnockout();
            }*/

            // Play the hit animation on the other player
            otherPlayer.GetComponent<Animator>().SetBool("HeadHit", true);

            // Knockback the other player (x-coord) with a force
            if (transform.forward.x > 0)
                otherPlayer.GetComponent<Rigidbody>().AddForce(new Vector3(10, 5, 0), ForceMode.Impulse);
            else
                otherPlayer.GetComponent<Rigidbody>().AddForce(new Vector3(-10, 5, 0), ForceMode.Impulse);

            crankedTimer = 10f; // Reset the timer
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

    IEnumerator CrankedTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            crankedTimer--;

            if (crankedTimer <= 0)
            {
                //isKnockedOut = true;
                //RagdollKnockout();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Debug.Log("OnTriggerEnter Grounded");
            isGrounded = true;
            rb.useGravity = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Debug.Log("OnTriggerStay Grounded");
            isGrounded = true;
            rb.useGravity = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Debug.Log("OnTriggerExit Grounded FALSE");
            isGrounded = false;
            rb.useGravity = true;
        }
    }
}
