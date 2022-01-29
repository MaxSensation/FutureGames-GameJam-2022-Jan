using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool followPlayer;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float zoomOffset;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float predictOffset;

    private Transform _playerTransform;
    private Vector3 _directionPrediction;

    private void Awake()
    {
        _directionPrediction = Vector3.zero;
    }

    private void Start()
    {
        GetPlayerTransform();
    }

    private void GetPlayerTransform()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (!followPlayer) return;
        if (!_playerTransform) _playerTransform = GameObject.FindWithTag("Player").transform;
        var newPos = Vector3.Lerp(
            transform.position, 
            _playerTransform.position + GetOffset(), 
            lerpSpeed * Time.deltaTime
        );
        newPos.z = zoomOffset;
        PredictMovement();
        transform.position = newPos;
    }

    private void PredictMovement()
    {
        _directionPrediction = (_playerTransform.position + verticalOffset * Vector3.up - transform.position).normalized;
    }

    private Vector3 GetOffset()
    {
        return verticalOffset * Vector3.up + predictOffset * _directionPrediction;
    }
}