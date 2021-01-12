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

public class CharacterControls : MonoBehaviour
{
    [SerializeField]
    float m_movementSpeed = 48f;
    [SerializeField]
    float m_distanceForEachStep = 16f;
    [SerializeField]
    float m_startWalkingDelay = 0.1f;

    Vector2 m_currentMovementVector = new Vector2();
    Vector2 m_movementStartingPoint = new Vector2();
    Rigidbody2D m_rigidBody = null;
    Animator m_animator = null;
    BoxCollider2D m_collider = null;
    bool m_controlsEnabled = true;
    bool m_isWalking = false;
    FacingDirection m_facingDirection = FacingDirection.South;
    float m_timePressingDirection = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.m_rigidBody = GetComponent<Rigidbody2D>();
        this.m_animator = GetComponent<Animator>();
        this.m_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ComputeControls();
        ComputeMovement();
        UpdateAnimatorParameters();
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
            if (hit.transform.tag != "Player")
            {
                Debug.Log(hit.transform.name);
                return false;
            }
        }
        return true;
    }

    private void ComputeControls()
    {
        if (!m_controlsEnabled) return;
        if (Input.GetAxis("Horizontal") > 0)
        {
            m_timePressingDirection += Time.deltaTime;
            if(m_timePressingDirection < m_startWalkingDelay){
                this.m_facingDirection = FacingDirection.East;
                return;
            }
            this.m_controlsEnabled = false;
            this.m_movementStartingPoint = this.transform.position;
            this.m_currentMovementVector = Vector2.right;
            this.m_isWalking = true;
            this.m_facingDirection = FacingDirection.East;
            return;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            m_timePressingDirection += Time.deltaTime;
            if(m_timePressingDirection < m_startWalkingDelay){
                this.m_facingDirection = FacingDirection.West;
                return;
            }
            this.m_controlsEnabled = false;
            this.m_movementStartingPoint = this.transform.position;
            this.m_currentMovementVector = Vector2.left;
            this.m_isWalking = true;
            this.m_facingDirection = FacingDirection.West;
            return;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            m_timePressingDirection += Time.deltaTime;
            if(m_timePressingDirection < m_startWalkingDelay){
                this.m_facingDirection = FacingDirection.North;
                return;
            }
            this.m_controlsEnabled = false;
            this.m_movementStartingPoint = this.transform.position;
            this.m_currentMovementVector = Vector2.up;
            this.m_isWalking = true;
            this.m_facingDirection = FacingDirection.North;
            return;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            m_timePressingDirection += Time.deltaTime;
            if(m_timePressingDirection < m_startWalkingDelay){
                this.m_facingDirection = FacingDirection.South;
                return;
            }
            this.m_controlsEnabled = false;
            this.m_movementStartingPoint = this.transform.position;
            this.m_currentMovementVector = Vector2.down;
            this.m_isWalking = true;
            this.m_facingDirection = FacingDirection.South;
            return;
        }
        this.m_timePressingDirection = 0.0f;
        this.m_isWalking = false;
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
                this.transform.position = new Vector3(this.m_movementStartingPoint.x - m_distanceForEachStep, this.transform.position.y, this.transform.position.z);
                this.m_controlsEnabled = true;
                // FIXME: Sometimes coordinates stop being aligned to a 16 unit grid (like 32.166). Maybe truncate value forcefully?
            }
        }
        if (this.m_currentMovementVector == Vector2.right)
        {
            if (this.transform.position.x > this.m_movementStartingPoint.x + m_distanceForEachStep)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(this.m_movementStartingPoint.x + m_distanceForEachStep, this.transform.position.y, this.transform.position.z);
                this.m_controlsEnabled = true;
            }

        }
        if (this.m_currentMovementVector == Vector2.up)
        {
            if (this.transform.position.y > this.m_movementStartingPoint.y + m_distanceForEachStep)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(this.transform.position.x, this.m_movementStartingPoint.y + m_distanceForEachStep, this.transform.position.z);
                this.m_controlsEnabled = true;
            }

        }
        if (this.m_currentMovementVector == Vector2.down)
        {
            if (this.transform.position.y < this.m_movementStartingPoint.y - m_distanceForEachStep)
            {
                this.m_currentMovementVector = new Vector2();
                this.m_rigidBody.velocity = new Vector2();
                this.transform.position = new Vector3(this.transform.position.x, this.m_movementStartingPoint.y - m_distanceForEachStep, this.transform.position.z);
                this.m_controlsEnabled = true;
            }

        }
    }
}
