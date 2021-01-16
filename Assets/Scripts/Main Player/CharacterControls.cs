using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: Character starts at x=241 y=-92 in new bark town.
[RequireComponent(typeof(CharacterMover))]
public class CharacterControls : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField]
    float m_startWalkingDelay = 0.1f;
    #endregion

    #region Internal Variables
    bool m_controlsEnabled = true;
    float m_timePressingDirection = 0.0f;
    #endregion

    #region Component refernces
    CharacterMover m_characterMover = null;
    #endregion

    private static CharacterControls _instance;
    public static CharacterControls Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null) throw new UnityException("There's already an instance of " + this.GetType().Name);
        _instance = this;

        m_characterMover = GetComponent<CharacterMover>();

    }

    void Start()
    {
    }

    void Update()
    {
        ComputeControls();
    }

    public void SetControlsEnabled(bool enabled)
    {
        this.m_controlsEnabled = enabled;
    }

    private void ComputeControls()
    {
        if (!m_controlsEnabled) return;

        Vector2 targetDirection = new Vector2();
        FacingDirection targetLookingDirection = FacingDirection.South;

        if (Input.GetButtonDown("A"))
        {
            m_characterMover.PerformInteraction();
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
            m_characterMover.SetWalking(false);
        }
        else
        {
            m_timePressingDirection += Time.deltaTime;
            m_characterMover.SetFacingDirection(targetLookingDirection);
        }

        // Ignore movement until a direction button was pressed long enough
        if (m_timePressingDirection < m_startWalkingDelay)
        {
            return;
        }

        // Check for collision to prevent moving if we can't reach the place
        if (!m_characterMover.CanPlayerMove(targetDirection)) return;

        // Start moving
        m_characterMover.StartMovingTowards(targetDirection);
        this.SetControlsEnabled(false);
    }


}
