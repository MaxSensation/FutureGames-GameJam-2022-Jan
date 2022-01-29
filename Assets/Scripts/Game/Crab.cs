using System.Collections;
using UnityEngine;

public class Crab : Enemy
{
    [SerializeField] private MovementType movementType;
    [SerializeField] private GameObject path;
    [SerializeField] private float changeWaypointDistance;
    [SerializeField] private float speed;
    
    
    private Transform[] _waypoints;
    private IEnumerator _waypointEnumerator;
    private bool _patrolling;
    private Transform _currentWayPoint;

    private void Start()
    {
        _waypoints = path.GetComponentsInChildren<Transform>();
        if (_waypoints.Length <= 1) return;
        _patrolling = true;
        _waypointEnumerator = _waypoints.GetEnumerator();
    }

    private void Update()
    {
        if (!_patrolling) return;
        if (_currentWayPoint == null || Vector2.Distance(transform.position, _currentWayPoint.position) <= changeWaypointDistance) ChangeWayPoint();
        MoveTowardsWayPoint();
    }

    private void MoveTowardsWayPoint()
    {
        switch (movementType)
        {
            case MovementType.Lerp:
                transform.position = Vector2.Lerp(transform.position, _currentWayPoint.position, speed * Time.deltaTime);
                break;
            case MovementType.Slerp:
                transform.position = Vector3.Slerp(transform.position, _currentWayPoint.position, speed * Time.deltaTime);
                break;
            case MovementType.Default:
                transform.position += (_currentWayPoint.position - transform.position).normalized * speed * Time.deltaTime;
                break;
        }
    }

    private void ChangeWayPoint()
    {
        if (!_waypointEnumerator.MoveNext())
        {
            _waypointEnumerator.Reset();
            _waypointEnumerator.MoveNext();
        }
        _currentWayPoint = (Transform)_waypointEnumerator.Current;
    }

    private enum MovementType
    {
        Default,
        Lerp,
        Slerp,
    }
}
