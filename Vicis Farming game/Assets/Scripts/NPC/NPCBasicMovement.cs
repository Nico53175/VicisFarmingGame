using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class NPCBasicMovement : MonoBehaviour
{
    public enum State
    {
        Idle,
        MovingLeft,
        MovingRight,
        MovingUp,
        MovingDown,
        Interaction
    }

    public float moveSpeed;
    public float maxDistance;
    private Rigidbody2D rb;
    public State currentState = State.Idle;
    private List<State> nonInteractionStates;
    private Vector2 spawnPoint;
    private CharacterAppearance characterAppearanceNPC;

    void Start()
    {
        //NPC Setup
        moveSpeed = 1.5f;
        maxDistance = 10f;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        spawnPoint = transform.position;
        characterAppearanceNPC = GetComponent<CharacterAppearance>();
        
        //State Setup
        nonInteractionStates = new List<State>((State[])Enum.GetValues(typeof(State)));
        nonInteractionStates.Remove(State.Interaction); // Exclude PlayerInteraction from the possible states.
        
        StartCoroutine(ChangeStatePeriodically());
    }

    void Update()
    {
        //Debug.Log("NPC " + name + " is currently in the " + currentState + " state.");
        switch (currentState)
        {
            case State.MovingLeft:
                rb.velocity = new Vector2(-moveSpeed, 0);
                break;
            case State.MovingRight:
                rb.velocity = new Vector2(moveSpeed, 0);
                break;
            case State.MovingUp:
                rb.velocity = new Vector2(0, moveSpeed);
                break;
            case State.MovingDown:
                rb.velocity = new Vector2(0, -moveSpeed);
                break;
            case State.Interaction:
                rb.velocity = Vector2.zero;
                break;
            default: // Idle state
                rb.velocity = Vector2.zero;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            Debug.Log($"{other.name} has entered {this.name} range. {this.name} is now in the Interaction state.");
            currentState = State.Interaction;
            characterAppearanceNPC.InteractionUpdate(other.transform);
            characterAppearanceNPC.isInteracting = true;
            if(other.gameObject.tag == "NPC")
            {
                other.GetComponent<CharacterAppearance>().InteractionUpdate(this.transform);
                other.GetComponent<NPCSpecificInteraction>().NPCInteraction();
                StartCoroutine(InteractionTimerNPC(other.transform));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            Debug.Log($"{other.name} has left {this.name} range. {this.name} is exiting the Interaction state.");
            // Stop the interaction timer when the other object leaves
            characterAppearanceNPC.isInteracting = false;
            StopAllCoroutines();
            StopInteraction();
        }
    }
    IEnumerator InteractionTimerNPC(Transform other)
    {
        // Wait for 5 to 10 seconds
        float waitTime = UnityEngine.Random.Range(5f, 10f);
        yield return new WaitForSeconds(waitTime);

        // After waiting, move NPCs away from each other
        Vector3 direction = (transform.position - other.position).normalized;
        float moveDistance = UnityEngine.Random.Range(2f, 4f); 
        float moveSpeed = 1.5f;
        float startTime = Time.time;

        // While the NPC has not reached the target position
        while ((Time.time - startTime) * moveSpeed < moveDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // End the interaction
        StopInteraction();
    }

    public void StopInteraction()
    {
        currentState = State.Idle;
        StartCoroutine(ChangeStatePeriodically());
    }

    void ChangeState()
    {
        if (currentState == State.Interaction)
        {
            return;
        }

        // Check if NPC is too far from the spawn point or interacting with the player
        if (Vector2.Distance(transform.position, spawnPoint) > maxDistance || currentState == State.Interaction)
        {
            // Choose the direction towards spawn point
            Vector2 direction = (spawnPoint - (Vector2)transform.position).normalized;

            if (Math.Abs(direction.x) > Math.Abs(direction.y))
            {
                // Move horizontally
                if (direction.x > 0)
                {
                    currentState = State.MovingRight;
                }
                else
                {
                    currentState = State.MovingLeft;
                }
            }
            else
            {
                // Move vertically
                if (direction.y > 0)
                {
                    currentState = State.MovingUp;
                }
                else
                {
                    currentState = State.MovingDown;
                }
            }
        }
        else
        {
            System.Random random = new System.Random();
            State randomState = nonInteractionStates[random.Next(nonInteractionStates.Count)]; // Select random state excluding PlayerInteraction
            currentState = randomState;
        }
    }

    IEnumerator ChangeStatePeriodically()
    {
        while (true)
        {
            ChangeState();
            // Generate a random float between 2 and 4 (for example)
            float randomInterval = UnityEngine.Random.Range(2f, 4f);
            yield return new WaitForSeconds(randomInterval);
        }
    }
}
