using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    GameObject otherPlayer;

    [SerializeField]
    bool isTestDummy;

    [SerializeField]
    Collider headbutt, head, body;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure the player is facing the otherPlayer
        if (otherPlayer.transform.position.x < transform.position.x)
        {
            transform.forward = new Vector3(-1, 0, 0);
        }
        else
        {
            transform.forward = new Vector3(1, 0, 0);
        }

        if (isTestDummy)
        {
            return;
        }

        // Relative movement controls using A and D keys. These forward and back controls are relative to the direction the player is facing.
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(0.01f, 0, 0);
            animator.SetBool("StepForward", true);

            // Print state of animator bool
            print("StepForward: " + animator.GetBool("StepForward"));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(-0.01f, 0, 0);
            animator.SetBool("StepBackward", true);
            // Print state of animator bool
            print("StepBackward: " + animator.GetBool("StepBackward"));
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

    // Runs partway through the headbutt animation, when the headbutt collider is in position to hit the other player
    public void OnHeadbuttHit()
    {
        // Check if headbutt collider hit the "Head" hitbox of the other player and the other player is not already in the "HeadHit" animation
        if (headbutt.bounds.Intersects(otherPlayer.GetComponent<PlayerController>().head.bounds) && !otherPlayer.GetComponent<Animator>().GetBool("HeadHit"))
        {
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

            // Stop the animation on this player
            animator.SetBool("Headbutt", false);
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

    /*private void OnCollisionEnter(Collision collision)
    {
        //print("Collision detected with " + collision.gameObject.name);

        // If headbutt is true, then check the collision
        if (animator.GetBool("Headbutt"))
        {
            // Check if this gameobject's box collider is colliding with the other player's box collider
            if (GetComponent<MeshCollider>().bounds.Intersects(otherPlayer.GetComponent<MeshCollider>().bounds))
            {
                print("Headbutt hit!");

                // Play the hit animation on the other player
                otherPlayer.GetComponent<Animator>().SetBool("HeadHit", true);
            }
        }
    }*/
}
