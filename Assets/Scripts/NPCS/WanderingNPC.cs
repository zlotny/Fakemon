using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMover))]
public class WanderingNPC : MonoBehaviour
{
    [SerializeField]
    private bool m_useOnlyYAxis = false;
    [SerializeField]
    private int m_maxHorizontalTilesToMove = 2;
    [SerializeField]
    private int m_maxVerticalTilesToMove = 2;

    [SerializeField]
    private float m_minSecondsToAction = 2.0f;
    [SerializeField]
    private float m_maxSecondsToAction = 6.0f;

    CharacterMover m_characterMover = null;
    private bool m_shouldMove = true;
    private Vector2 m_originalPosition;

    void Awake()
    {
        m_characterMover = GetComponent<CharacterMover>();
    }

    void Start()
    {
        m_characterMover.SetMovingCompleteCallback(StopWalking);
        UIDialogPanel.Instance.SetFinishDialogCallback(ResumeMoving);
        m_originalPosition = this.transform.position;
        StartCoroutine(EachSecond());
    }

    void MoveRandomly()
    {
        if (!m_shouldMove) return;
        FacingDirection targetFacingDirection = FacingDirection.South;
        int randomChoice = Random.Range(0, m_useOnlyYAxis ? 2 : 4);
        if (randomChoice == 0) targetFacingDirection = FacingDirection.North;
        if (randomChoice == 1) targetFacingDirection = FacingDirection.South;
        if (randomChoice == 2) targetFacingDirection = FacingDirection.West;
        if (randomChoice == 3) targetFacingDirection = FacingDirection.East;

        Vector2 targetVector = m_characterMover.FacingDirectionToVector(targetFacingDirection);
        if (!m_characterMover.CanNPCMove(targetVector)) return;
        m_characterMover.SetFacingDirection(targetFacingDirection);
        if (!SatisfiesMovementConstraints(targetVector)) return;
        m_characterMover.StartMovingTowards(targetVector);
    }

    public bool SatisfiesMovementConstraints(Vector2 targetDestination)
    {
        if (targetDestination == Vector2.up || targetDestination == Vector2.down)
        {
            float targetDestinationYPosition = transform.position.y + (targetDestination.y * m_characterMover.GetDistanceForEachStep());
            if (Mathf.Abs(targetDestinationYPosition - m_originalPosition.y) <= m_characterMover.GetDistanceForEachStep() * m_maxVerticalTilesToMove)
            {
                return true;
            }
        }
        if (targetDestination == Vector2.right || targetDestination == Vector2.left)
        {
            float targetDestinationXPosition = transform.position.x + (targetDestination.x * m_characterMover.GetDistanceForEachStep());
            if (Mathf.Abs(targetDestinationXPosition - m_originalPosition.x) <= m_characterMover.GetDistanceForEachStep() * m_maxHorizontalTilesToMove)
            {
                return true;
            }
        }
        return false;
    }

    public void StopMoving()
    {
        this.m_shouldMove = false;
    }

    public void ResumeMoving()
    {
        this.m_shouldMove = true;
    }

    public void FaceTo(Vector2 vector)
    {
        m_characterMover.SetFacingDirection(m_characterMover.VectorToFacingDirection(vector));
    }

    IEnumerator EachSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(this.m_minSecondsToAction, this.m_maxSecondsToAction));
            MoveRandomly();
        }
    }

    void StopWalking()
    {
        m_characterMover.SetWalking(false);
    }

    void Update()
    {

    }
}
