using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class CharacterMover : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField]
    float m_movementSpeed = 64f;
    [SerializeField]
    float m_distanceForEachStep = 16f;
    #endregion

    #region Component refernces
    Rigidbody2D m_rigidBody = null;
    Animator m_animator = null;
    BoxCollider2D m_collider = null;
    CharacterControls m_characterControls = null;
    #endregion

    #region Internal Variables
    FacingDirection m_facingDirection = FacingDirection.South;
    Vector2 m_currentMovementVector = new Vector2();
    Vector2 m_movementStartingPoint = new Vector2();
    Vector2 m_nextTeleportPoint = new Vector2();
    bool m_isWalking = false;
    bool m_isJumping = false;
    bool m_needsToTeleportASAP = false;
    Action m_onMovingCompleteCallback;
    #endregion

    void Awake()
    {
        this.m_rigidBody = GetComponent<Rigidbody2D>();
        this.m_animator = GetComponent<Animator>();
        this.m_collider = GetComponent<BoxCollider2D>();
        this.m_characterControls = GetComponent<CharacterControls>();
    }

    // Update is called once per frame
    void Update()
    {
        ComputeMovement();
        UpdateAnimatorParameters();
        CheckIfNeedTeleport();
    }

    public void SetMovingCompleteCallback(Action callback)
    {
        m_onMovingCompleteCallback += callback;
    }

    public void SetWalking(bool newState)
    {
        this.m_isWalking = newState;
    }

    public float GetDistanceForEachStep()
    {
        return m_distanceForEachStep;
    }

    public FacingDirection GetFacingDirection()
    {
        return m_facingDirection;
    }

    public void SetFacingDirection(FacingDirection newState)
    {
        this.m_facingDirection = newState;
    }

    public void StartMovingTowards(Vector2 direction)
    {
        this.m_movementStartingPoint = this.transform.position;
        this.m_currentMovementVector = direction;
        SetWalking(true);
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
        this.m_animator.SetBool("IsJumping", this.m_isJumping);
        this.m_animator.SetInteger("FacingDirection", (int)this.m_facingDirection);
    }

    public bool CanNPCMove(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.m_collider.bounds.center, direction, m_distanceForEachStep * 1.2f);
        foreach (var hit in hits)
        {
            if (hit.transform.tag != "NPC" && !hit.collider.isTrigger)
            {
                return false;
            }
            else
            {
                bool isPlayer = hit.transform.tag == "Player";
                if (isPlayer) return false;
            }
        }
        return true;
    }

    public bool CanPlayerMove(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.m_collider.bounds.center, direction, m_distanceForEachStep * 1.2f);
        foreach (var hit in hits)
        {
            if (hit.transform.tag != "Player" && !hit.collider.isTrigger)
            {
                return false;
            }
            else
            {
                bool isNPC = hit.transform.tag == "NPC";
                // FIXME: This is a tad stupid but... if it's another player? Maybe check that?
                if (isNPC) return false;
            }
        }
        return true;
    }

    public bool CanPlayerJump(FacingDirection facingDirection)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.m_collider.bounds.center, FacingDirectionToVector(facingDirection), m_distanceForEachStep * 1.2f);
        foreach (var hit in hits)
        {
            Ledge ledgeComponent = hit.collider.GetComponent<Ledge>();
            if (ledgeComponent == null) continue;
            if (ledgeComponent.GetJumpableDirection() == this.m_facingDirection)
            {
                return true;
            }
        }
        return false;
    }

    public void JumpTowards(Vector2 direction)
    {
        if (m_characterControls) m_characterControls.SetControlsEnabled(false);
        m_collider.enabled = false;
        m_isJumping = true;
        StartMovingTowards(direction);
    }

    public void StopJumping()
    {
        m_isJumping = false;
        m_collider.enabled = true;
        if (m_characterControls) m_characterControls.SetControlsEnabled(true);
    }

    public void PerformInteraction()
    {
        Vector2 direction = FacingDirectionToVector(m_facingDirection);
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.m_collider.bounds.center, direction, m_distanceForEachStep);
        foreach (var hit in hits)
        {
            Interactable targetInteractable = hit.transform.gameObject.GetComponent<Interactable>();
            if (targetInteractable == null)
                continue;
            targetInteractable.Interact(this.gameObject);
        }
    }

    public Vector2 FacingDirectionToVector(FacingDirection facingDirection)
    {
        if (facingDirection == FacingDirection.North) return Vector2.up;
        if (facingDirection == FacingDirection.East) return Vector2.right;
        if (facingDirection == FacingDirection.South) return Vector2.down;
        if (facingDirection == FacingDirection.West) return Vector2.left;
        throw new UnityException("Weird cardinal direction received :S: " + facingDirection);
    }

    public FacingDirection VectorToFacingDirection(Vector2 vector)
    {
        if (vector == Vector2.up) return FacingDirection.North;
        if (vector == Vector2.right) return FacingDirection.East;
        if (vector == Vector2.down) return FacingDirection.South;
        if (vector == Vector2.left) return FacingDirection.West;
        throw new UnityException("Weird vector received :S: " + vector);
    }

    private void ComputeMovement()
    {
        this.m_rigidBody.velocity = m_currentMovementVector * m_movementSpeed;
        float totalDistanceToMove = m_isJumping ? m_distanceForEachStep * 2 : m_distanceForEachStep;
        if (this.m_currentMovementVector == Vector2.left)
        {
            if (this.transform.position.x < this.m_movementStartingPoint.x - totalDistanceToMove)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(Mathf.Floor(this.m_movementStartingPoint.x - totalDistanceToMove), Mathf.Floor(this.transform.position.y), Mathf.Floor(this.transform.position.z));
                if (m_isJumping) StopJumping();
                if (m_onMovingCompleteCallback != null) m_onMovingCompleteCallback();
                if (m_characterControls) m_characterControls.SetControlsEnabled(true);
            }
        }
        if (this.m_currentMovementVector == Vector2.right)
        {
            if (this.transform.position.x > this.m_movementStartingPoint.x + totalDistanceToMove)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(Mathf.Floor(this.m_movementStartingPoint.x + totalDistanceToMove), Mathf.Floor(this.transform.position.y), Mathf.Floor(this.transform.position.z));
                if (m_isJumping) StopJumping();
                if (m_onMovingCompleteCallback != null) m_onMovingCompleteCallback();
                if (m_characterControls) m_characterControls.SetControlsEnabled(true);
            }

        }
        if (this.m_currentMovementVector == Vector2.up)
        {
            if (this.transform.position.y > this.m_movementStartingPoint.y + totalDistanceToMove)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(Mathf.Floor(this.transform.position.x), Mathf.Floor(this.m_movementStartingPoint.y + totalDistanceToMove), Mathf.Floor(this.transform.position.z));
                if (m_isJumping) StopJumping();
                if (m_onMovingCompleteCallback != null) m_onMovingCompleteCallback();
                if (m_characterControls) m_characterControls.SetControlsEnabled(true);
            }

        }
        if (this.m_currentMovementVector == Vector2.down)
        {
            if (this.transform.position.y < this.m_movementStartingPoint.y - totalDistanceToMove)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(Mathf.Floor(this.transform.position.x), Mathf.Floor(this.m_movementStartingPoint.y - totalDistanceToMove), Mathf.Floor(this.transform.position.z));
                if (m_isJumping) StopJumping();
                if (m_onMovingCompleteCallback != null) m_onMovingCompleteCallback();
                if (m_characterControls) m_characterControls.SetControlsEnabled(true);
            }

        }
    }
}
