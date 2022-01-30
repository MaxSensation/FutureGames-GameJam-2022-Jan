using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector2 _lastCheckpointPos;
    private void Start()
    {
        Checkpoint.OnCheckPointReachedEvent += ReachedCheckpoint;
        SquidController.OnDiedEvent += LoadCheckPoint;
    }

    private void ReachedCheckpoint(Vector2 newPos) => _lastCheckpointPos = newPos;

    private void LoadCheckPoint()
    {
        transform.position = _lastCheckpointPos;
    }
}
