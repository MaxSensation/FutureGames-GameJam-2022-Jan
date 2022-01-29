using System;
using MaxHelpers;
using SquidStates;
using UnityEngine;

public class SquidController : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private float squirtStrength = 100f;
    [SerializeField] private float leavingWaterBoost = 100f;
    [SerializeField] private InAirParams inAirParams;
    [SerializeField] private InWaterParams inWaterParams;
    public Rigidbody2D Rb { get; private set; }
    private readonly StateMachine _stateMachine = new ();
    
    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        var underwater = new SquidUnderwaterState(this, inWaterParams);
        var air = new SquidAirState(this, inAirParams);
        var leavingWater = new SquidLeavingWaterState(this, leavingWaterBoost);
        var squirt = new SquidSquirtState(this, squirtStrength);
        var ground = new SquidGroundState(this);
        _stateMachine.AddAnyTransition(squirt, CanWaterSquirt);
        _stateMachine.AddTransition(squirt, air, () => !_isGrounded && !_isUnderwater);
        _stateMachine.AddTransition(squirt, underwater, () => !_isGrounded && _isUnderwater);
        _stateMachine.AddTransition(squirt, underwater, () => _isGrounded && !_isUnderwater);
        _stateMachine.AddTransition(ground, air, () => !_isGrounded && !_isUnderwater);
        _stateMachine.AddTransition(air, ground, () => _isGrounded && !_isUnderwater);
        _stateMachine.AddTransition(underwater, leavingWater, () => !_isUnderwater && !_isGrounded);
        _stateMachine.AddTransition(leavingWater, air, () => true);
        _stateMachine.AddTransition(air, underwater, () => _isUnderwater);
        _stateMachine.SetState(underwater);
        Rb.gravityScale = 4f;
        if (debugMode) GameManager.Instance.Inputs.Player.Enable();
    }

    private bool CanWaterSquirt()
    {
        return GameManager.Instance.Inputs.Player.Jump.WasPressedThisFrame();
    }

    private void Update()
    {
        CheckGround();
        CheckUnderWater();
        _stateMachine.Tick();
    }
    
    public void RotateTowards(float speed, bool velocity = false)
    {
        var dir = velocity ? Rb.velocity : GameManager.Instance.Inputs.Player.Move.ReadValue<Vector2>();
        if (!(dir.magnitude > 0f)) return;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var newRot = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle -90f, Vector3.forward), speed * Time.deltaTime);
        transform.rotation = newRot;
    }
    public void HandleMovement(float speed, float acceleration, float control = 1f, bool yControl = true)
    {
        var inputs = GameManager.Instance.Inputs.Player.Move.ReadValue<Vector2>();
        inputs = Vector2.MoveTowards(inputs, inputs.normalized, control * acceleration * Time.deltaTime);
        var idealVel = new Vector3(inputs.x * speed, inputs.y * speed);
        if (!yControl) idealVel.y = Rb.velocity.y;
        var velocity = Rb.velocity;
        Rb.velocity = Vector3.MoveTowards(velocity, Vector2.Lerp(velocity, idealVel, control), control * 100f * Time.deltaTime);
    }

    #region Detection
    
    [Header("Detection")] 
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundOffsetDistance = 0.5f;
    [SerializeField] private float waterCheckRadius = 0.2f;
    private bool _isGrounded, _isUnderwater;
    private readonly Collider2D[] _ground = new Collider2D[1];
    private readonly Collider2D[] _water = new Collider2D[1];

    private void CheckUnderWater() => _isUnderwater = Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, _water, waterMask) > 0;
    private void CheckGround() => _isGrounded = Physics2D.OverlapCircleNonAlloc(transform.position + Vector3.down * groundOffsetDistance, groundCheckRadius, _ground, groundMask) > 0;

    #endregion

    #region Debug
    private void DrawGrounderGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundOffsetDistance, groundCheckRadius);
    }
    
    private void DrawWaterGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, waterCheckRadius);
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;
        DrawGrounderGizmos();
        DrawWaterGizmos();
    }
    #endregion
    
    [Serializable]
    public struct InAirParams
    {
        public  float speed;
        public  float acceleration;
        public  float control;
        public  float rotationSpeed;
        public bool rotateVelocity;
    }
    
    [Serializable]
    public struct InWaterParams
    {
        public float speed;
        public  float acceleration;
        public  float rotationSpeed;
    }
}