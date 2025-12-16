using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SegmentManager : MonoBehaviour
{
    [SerializeField] private GameObject[] segments;

    Vector2[] octagonDirections = new Vector2[]
    {
        Vector2.up,
        (Vector2.up + Vector2.right).normalized,
        Vector2.right,
        (Vector2.down + Vector2.right).normalized,
        Vector2.down,
        (Vector2.down + Vector2.left).normalized,
        Vector2.left,
        (Vector2.up + Vector2.left).normalized
    };

    private InputAction _moveAction;

    private int _currentCommittedSegment = -1;
    private int _pendingSegment = -1;
    private Coroutine _stabilizeRoutine;
    private float _spawnDistanceFromCentre = 7f;

    [SerializeField] private float directionStabilizeDelay = 0.07f;

    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");

        for (int i = 0; i < segments.Length; i++)
            segments[i].SetActive(false);
    }

    private void Update()
    {
        HandleInput();
    }

    int GetSegmentFromVector(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return -1;

        float maxDot = -Mathf.Infinity;
        int bestIndex = 0;

        for (int i = 0; i < octagonDirections.Length; i++)
        {
            float dot = Vector2.Dot(dir, octagonDirections[i]);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    private void HandleInput()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();
        int rawIndex = GetSegmentFromVector(input);

        if (rawIndex == _pendingSegment)
            return;

        _pendingSegment = rawIndex;

        if (_stabilizeRoutine != null)
            StopCoroutine(_stabilizeRoutine);

        _stabilizeRoutine = StartCoroutine(StabilizeDirection(rawIndex));
    }

    private IEnumerator StabilizeDirection(int targetSegment)
    {
        float t = 0f;

        while (t < directionStabilizeDelay)
        {
            var latest = GetSegmentFromVector(_moveAction.ReadValue<Vector2>());
            if (latest != targetSegment)
                yield break;

            t += Time.deltaTime;
            yield return null;
        }

        CommitSegment(targetSegment);
    }

    private void CommitSegment(int index)
    {
        if (_currentCommittedSegment != -1)
            segments[_currentCommittedSegment].SetActive(false);

        if (index != -1)
            segments[index].SetActive(true);

        _currentCommittedSegment = index;
    }
    
    
    public Vector2 GetWorldPositionForSide(int side)
    {
        Vector2 dir = octagonDirections[side];
        return (Vector2)transform.position + dir * _spawnDistanceFromCentre; 
    }

}
