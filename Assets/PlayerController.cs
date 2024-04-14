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

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = Vector3.up * 5;
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
                otherPlayer.GetComponent<Rigidbody>().AddForce(new Vector3(5, 5, 0), ForceMode.Impulse);
            else
                otherPlayer.GetComponent<Rigidbody>().AddForce(new Vector3(-5, 5, 0), ForceMode.Impulse);

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

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            Debug.Log("OnStayCollisionEnter Grounded");
            isGrounded = true;
            rb.useGravity = false;
            animator.enabled = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            Debug.Log("OnCollisionExit Grounded FALSE");
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
            yield return new WaitForSeconds(0.1f);
            animator.enabled = false;
            Debug.Log("Ragdolling...");
        }
    }
}
