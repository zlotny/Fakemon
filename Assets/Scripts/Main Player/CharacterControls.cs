using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: Character starts at x=241 y=-92 in new bark town.

public enum FacingDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterControls : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField]
    float m_movementSpeed = 48f;
    [SerializeField]
    float m_distanceForEachStep = 16f;
    [SerializeField]
    float m_startWalkingDelay = 0.1f;
    #endregion

    #region Component refernces
    Rigidbody2D m_rigidBody = null;
    Animator m_animator = null;
    BoxCollider2D m_collider = null;
    #endregion

    #region Internal Variables
    FacingDirection m_facingDirection = FacingDirection.South;
    Vector2 m_currentMovementVector = new Vector2();
    Vector2 m_movementStartingPoint = new Vector2();
    Vector2 m_nextTeleportPoint = new Vector2();

    bool m_controlsEnabled = true;
    bool m_isWalking = false;
    bool m_needsToTeleportASAP = false;

    float m_timePressingDirection = 0.0f;
    #endregion

    void Start()
    {
        this.m_rigidBody = GetComponent<Rigidbody2D>();
        this.m_animator = GetComponent<Animator>();
        this.m_collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        ComputeControls();
        ComputeMovement();
        UpdateAnimatorParameters();
        CheckIfNeedTeleport();
    }

    public void RequestTeleportToPoint(Vector2 destination)
    {
        this.m_nextTeleportPoint = destination;
        this.m_needsToTeleportASAP = true;
    }

    private void CheckIfNeedTeleport()
    {
        if (IsIdle())
        {
            if (this.m_needsToTeleportASAP)
            {
                this.transform.position = this.m_nextTeleportPoint;
                this.m_needsToTeleportASAP = false;
                this.m_nextTeleportPoint = new Vector2();
            }
        }
    }

    private bool IsIdle()
    {
        return this.m_rigidBody.velocity.magnitude == 0;
    }

    private void UpdateAnimatorParameters()
    {
        this.m_animator.SetBool("IsWalking", this.m_isWalking);
        this.m_animator.SetInteger("FacingDirection", (int)this.m_facingDirection);
    }

    private bool CanMove(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.m_collider.bounds.center, direction, m_distanceForEachStep);
        foreach (var hit in hits)
        {
            if (hit.transform.tag != "Player" && !hit.collider.isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    private void PerformInteraction(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.m_collider.bounds.center, direction, m_distanceForEachStep);
        foreach (var hit in hits)
        {
            Interactable targetInteractable = hit.transform.gameObject.GetComponent<Interactable>();
            if (targetInteractable == null)
                return;
            targetInteractable.Interact(this.gameObject);
        }
    }

    private Vector2 FacingDirectionToVector(FacingDirection facingDirection)
    {
        if (facingDirection == FacingDirection.North) return Vector2.up;
        if (facingDirection == FacingDirection.East) return Vector2.right;
        if (facingDirection == FacingDirection.South) return Vector2.down;
        if (facingDirection == FacingDirection.West) return Vector2.left;
        throw new UnityException("Weird cardinal direction received :S: " + facingDirection);
    }

    private void ComputeControls()
    {
        if (!m_controlsEnabled) return;

        Vector2 targetDirection = new Vector2();
        FacingDirection targetLookingDirection = FacingDirection.South;

        if (Input.GetButtonDown("A"))
        {
            PerformInteraction(FacingDirectionToVector(m_facingDirection));
        }

        if (Input.GetButtonDown("B"))
        {
            Debug.Log("Player pressed B ");
        }

        // Set the direction we want to go
        if (Input.GetAxis("Horizontal") > 0)
        {
            targetDirection = Vector2.right;
            targetLookingDirection = FacingDirection.East;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            targetDirection = Vector2.left;
            targetLookingDirection = FacingDirection.West;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            targetDirection = Vector2.up;
            targetLookingDirection = FacingDirection.North;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            targetDirection = Vector2.down;
            targetLookingDirection = FacingDirection.South;
        }

        // Increments the timer for pressing a direction button to generate a delay
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            this.m_timePressingDirection = 0.0f;
            this.m_isWalking = false;
        }
        else
        {
            m_timePressingDirection += Time.deltaTime;
            this.m_facingDirection = targetLookingDirection;
        }

        // Ignore movement until a direction button was pressed long enough
        if (m_timePressingDirection < m_startWalkingDelay)
        {
            return;
        }

        // Check for collision to prevent moving if we can't reach the place
        if (!CanMove(targetDirection)) return;

        // Start moving
        this.m_controlsEnabled = false;
        this.m_movementStartingPoint = this.transform.position;
        this.m_currentMovementVector = targetDirection;
        this.m_isWalking = true;
    }

    private void ComputeMovement()
    {
        this.m_rigidBody.velocity = m_currentMovementVector * m_movementSpeed;
        if (this.m_currentMovementVector == Vector2.left)
        {
            if (this.transform.position.x < this.m_movementStartingPoint.x - m_distanceForEachStep)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(Mathf.Floor(this.m_movementStartingPoint.x - m_distanceForEachStep), Mathf.Floor(this.transform.position.y), Mathf.Floor(this.transform.position.z));
                this.m_controlsEnabled = true;
            }
        }
        if (this.m_currentMovementVector == Vector2.right)
        {
            if (this.transform.position.x > this.m_movementStartingPoint.x + m_distanceForEachStep)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(Mathf.Floor(this.m_movementStartingPoint.x + m_distanceForEachStep), Mathf.Floor(this.transform.position.y), Mathf.Floor(this.transform.position.z));
                this.m_controlsEnabled = true;
            }

        }
        if (this.m_currentMovementVector == Vector2.up)
        {
            if (this.transform.position.y > this.m_movementStartingPoint.y + m_distanceForEachStep)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(Mathf.Floor(this.transform.position.x), Mathf.Floor(this.m_movementStartingPoint.y + m_distanceForEachStep), Mathf.Floor(this.transform.position.z));
                this.m_controlsEnabled = true;
            }

        }
        if (this.m_currentMovementVector == Vector2.down)
        {
            if (this.transform.position.y < this.m_movementStartingPoint.y - m_distanceForEachStep)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(Mathf.Floor(this.transform.position.x), Mathf.Floor(this.m_movementStartingPoint.y - m_distanceForEachStep), Mathf.Floor(this.transform.position.z));
                this.m_controlsEnabled = true;
            }

        }
    }
}
