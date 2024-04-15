using System.Collections;
using UnityEngine;
using UnityEngine.AI;
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
    Text forceMultiplierText, livesLeftText;

    [SerializeField]
    GameObject crankedTimer;

    [SerializeField]
    int lives = 3;

    [SerializeField]
    bool isPlayer1;

    int forceMultiplier = 1;
    public bool isGrounded = false;
    float timeSinceLastAttack = 0f;
    bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        livesLeftText.text = $"{lives} LIVES LEFT";
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePaused) return;
        if (isTestDummy) return; // For debugging

        timeSinceLastAttack += Time.deltaTime;

        // If there is 10 seconds left and not already counting down, show the timer
        if (timeSinceLastAttack >= 10f && crankedTimer.GetComponent<CrankedTimer>().countingDown == false)
        {
            crankedTimer.GetComponent<CrankedTimer>().countingDown = true;
        }

        // Make sure the player is facing the otherPlayer
        // Calculate the direction vector from this player to the other player
        Vector3 directionToOtherPlayer = otherPlayer.transform.position - transform.position;

        // Remove any vertical component from the direction vector
        directionToOtherPlayer.y = 0;

        // Set this player's forward direction to the direction vector
        transform.forward = directionToOtherPlayer;

        if (isPlayer1)
        {
            // PLAYER 1 CONTROLS
            // Relative movement controls using A and D keys. These forward and back controls are relative to the direction the player is facing.
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(-0.02f, 0, 0);
                animator.SetBool("IsMoving", true);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(0.02f, 0, 0);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            // Jumping
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                rb.velocity = Vector3.up * 10;
                animator.SetBool("IsJumping", true);
                isGrounded = false;
            }
            if (isGrounded)
            {
                animator.SetBool("IsJumping", false);
            }

            // Attack
            if (Input.GetKeyDown(KeyCode.S))
            {
                animator.SetBool("Headbutt", true);
            }
        }
        else
        {
            // PLAYER 2 CONTROLS
            // Relative movement controls using J and L keys. These forward and back controls are relative to the direction the player is facing.
            if (Input.GetKey(KeyCode.L))
            {
                transform.position += new Vector3(-0.02f, 0, 0);
                animator.SetBool("IsMoving", true);
            }
            else if (Input.GetKey(KeyCode.J))
            {
                transform.position += new Vector3(0.02f, 0, 0);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            // Jumping
            if (Input.GetKeyDown(KeyCode.I) && isGrounded)
            {
                rb.velocity = Vector3.up * 10;
                animator.SetBool("IsJumping", true);
                isGrounded = false;
            }
            if (isGrounded)
            {
                animator.SetBool("IsJumping", false);
            }

            // Headbutt
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetBool("Headbutt", true);
            }
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
                otherPlayer.GetComponent<Rigidbody>().AddForce(new Vector3(3, 1, 0) * forceMultiplier, ForceMode.Impulse);
            else
                otherPlayer.GetComponent<Rigidbody>().AddForce(new Vector3(-3, 1, 0) * forceMultiplier, ForceMode.Impulse);

            forceMultiplier++; // Increase the force for the next headbutt
            forceMultiplierText.text = forceMultiplier + "x FORCE"; // Update the UI text

            timeSinceLastAttack = 0f; // Reset the timer
            crankedTimer.GetComponent<CrankedTimer>().ResetTimer(); // Reset the timer
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

    // Player touches collider
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.useGravity = false;
            animator.enabled = true;
        }

        if (other.collider.CompareTag("Killzone"))
        {
            KnockOut();
        }
    }

    // Player no longer touching collider
    void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = false;
            rb.useGravity = true;
            //StartCoroutine(AirTimeRagdoll());
        }
    }

    // If the player is in the air for a certain amount of time, then ragdoll them
    IEnumerator AirTimeRagdoll()
    {
        while (!isGrounded)
        {
            yield return new WaitForSeconds(2f);
            animator.enabled = false;
            Debug.Log("Ragdolling...");
        }

        animator.enabled = true;
    }

    // Player is knocked out
    public void KnockOut()
    {
        lives--;
        livesLeftText.text = $"{lives} LIVES LEFT";

        // If this player has no more lives
        // First check if both players have no more lives
        if (lives <= 0 && otherPlayer.GetComponent<PlayerController>().lives <= 0)
        {
            gamePaused = true;
            Time.timeScale = 0; // Pause the game

            // Find the WinText object and modify the text
            GameObject.Find("WinText").GetComponent<Text>().text = "YOU BOTH SUCK";
            return;
        }
        else if (lives <= 0)
        {
            gamePaused = true;
            Time.timeScale = 0; // Pause the game

            // Find the WinText object and modify the text
            if (isPlayer1)
            {
                GameObject.Find("WinText").GetComponent<Text>().text = "PLAYER 2 WINS";
            }
            else
            {
                GameObject.Find("WinText").GetComponent<Text>().text = "PLAYER 1 WINS";
            }
            return;
        }

        // Respawn the player by dropping them above the stage (absolute world position)
        transform.position = new Vector3(0, 6, 0);
    }
}
