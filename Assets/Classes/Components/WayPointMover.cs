using System.Collections.Generic;
using UnityEngine;

public class WayPointMover : MonoBehaviour
{
    enum MoveState
    {
        PingPong,
        Loop,
        Once
    }

    [SerializeField] private List<Vector2> wayPoints;
    [SerializeField] private float speed = 10f;
    [SerializeField] private MoveState moveState;

    private int currentIndex = 0;
    private int direction = 1; // 1 forward, -1 backward
    private bool isDone = false;

    private void Awake()
    {
        if (wayPoints == null || wayPoints.Count < 2)
        {
            Debug.LogError("Need at least 2 waypoints");
            enabled = false;
            return;
        }

        transform.position = wayPoints[0];
    }

    private void Update()
    {
        if (isDone) return;

        transform.position = Vector2.MoveTowards(transform.position, 
                                                wayPoints[currentIndex], 
                                                speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, wayPoints[currentIndex]) < 0.01f)
            AdvanceIndex();

    }

    private void AdvanceIndex()
    {
        switch (moveState)
        {
            case MoveState.Once:
                HandleOnce();
                break;

            case MoveState.Loop:
                HandleLoop();
                break;

            case MoveState.PingPong:
                HandlePingPong();
                break;
        }
    }

    // ===== STATE HANDLERS =====

    private void HandleOnce()
    {
        if ((direction == 1 && currentIndex >= wayPoints.Count - 1) ||
            (direction == -1 && currentIndex <= 0))
        {
            isDone = true;
            return;
        }

        currentIndex += direction;
    }

    private void HandleLoop()
    {
        currentIndex += direction;

        if (currentIndex >= wayPoints.Count)
            currentIndex = 0;
        else if (currentIndex < 0)
            currentIndex = wayPoints.Count - 1;
    }

    private void HandlePingPong()
    {
        if (currentIndex == wayPoints.Count - 1)
            direction = -1;
        else if (currentIndex == 0)
            direction = 1;

        currentIndex += direction;
    }
}