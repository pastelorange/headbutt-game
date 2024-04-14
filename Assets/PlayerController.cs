using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    [SerializeField]
    GameObject otherPlayer;

    [SerializeField]
    bool isTestDummy;

    [SerializeField]
    Collider headCollider;

    [SerializeField]
    Text forceMultiplierText;

    [SerializeField]
    GameObject crankedTimer;

    int forceMultiplier = 1;
    bool isKnockedOut;
    bool isGrounded = false;
    float timeSinceLastAttack = 0f;


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

        timeSinceLastAttack += Time.deltaTime;

        // If there is 10 seconds left and not already counting down, show the timer
        if (timeSinceLastAttack >= 10f && crankedTimer.GetComponent<CrankedTimer>().countingDown == false)
        {
            crankedTimer.GetComponent<CrankedTimer>().countingDown = true;
        }

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
            transform.position += new Vector3(0.03f, 0, 0);
            animator.SetBool("IsMoving", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(-0.03f, 0, 0);
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

            // TODO: Freeze transform position while headbutting
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = Vector3.up * 10;
            animator.SetBool("IsJumping", true);
            isGrounded = false;
        }
        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
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

            // Play the hit animation on the other player
            otherPlayer.GetComponent<Animator>().SetBool("HeadHit", true);

            // Knockback the other player (x-coord) with a force
            if (transform.forward.x > 0)
                otherPlayer.GetComponent<Rigidbody>().AddForce(new Vector3(2, 1, 0) * forceMultiplier, ForceMode.Impulse);
            else
                otherPlayer.GetComponent<Rigidbody>().AddForce(new Vector3(-2, 1, 0 * forceMultiplier), ForceMode.Impulse);

            forceMultiplier++; // Increase the force for the next headbutt
            forceMultiplierText.text = forceMultiplier + "x"; // Update the UI text

            timeSinceLastAttack = 0f; // Reset the timer
            crankedTimer.GetComponent<CrankedTimer>().countingDown = false; // Hide the UI timer
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

    // Player is on ground
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.useGravity = false;
            animator.enabled = true;
        }
    }

    // Player is in the air
    void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = false;
            rb.useGravity = true;
            StartCoroutine(AirTimeRagdoll());
        }
    }

    // If the player is in the air for a certain amount of time, then ragdoll them
    IEnumerator AirTimeRagdoll()
    {
        while (!isGrounded)
        {
            yield return new WaitForSeconds(1f);
            animator.enabled = false;
            Debug.Log("Ragdolling...");
        }

        animator.enabled = true;
    }

    // Player is knocked out
    public void KnockOut()
    {
        isKnockedOut = true;
        Debug.Log("Player is knocked out");
    }
}
