using System;
using MaxHelpers;
using SquidStates;
using UnityEngine;

public class SquidController : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private float inWaterSquirtStrength;
    [SerializeField] private float otherStatesWaterStrength;
    [SerializeField] private InAirParams inAirParams;
    [SerializeField] private InWaterParams inWaterParams;
    [SerializeField] private LeavingWaterParams leavingWaterParams;
    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    
    private readonly StateMachine _stateMachine = new ();

    private void Start()
    {
        WaterLevel = 1f;
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        var underwater = new SquidSwimState(this, inWaterParams);
        var air = new SquidAirState(this, inAirParams);
        var leavingWater = new SquidLeavingWaterState(this, leavingWaterParams, inWaterParams);
        var squirt = new SquidSquirtState(this, inWaterSquirtStrength);
        var ground = new SquidGroundState(this);
        var death = new SquidDeathState(this);
        _stateMachine.AddTransition(air, death, () => _isGrounded && WaterLevel <= 0f);
        _stateMachine.AddTransition(ground, death, () => _isGrounded && WaterLevel <= 0f);
        _stateMachine.AddAnyTransition(squirt, CanWaterSquirt);
        _stateMachine.AddTransition(squirt, air, () => !_isGrounded && !_isUnderwater);
        _stateMachine.AddTransition(squirt, underwater, () => !_isGrounded && _isUnderwater);
        _stateMachine.AddTransition(squirt, ground, () => _isGrounded && !_isUnderwater);
        _stateMachine.AddTransition(ground, air, () => !_isGrounded && !_isUnderwater);
        _stateMachine.AddTransition(air, ground, () => _isGrounded && !_isUnderwater);
        _stateMachine.AddTransition(underwater, leavingWater, () => !_isUnderwater && !_isGrounded);
        _stateMachine.AddTransition(leavingWater, air, () => true);
        _stateMachine.AddTransition(air, underwater, () => _isUnderwater);
        _stateMachine.SetState(underwater);
        squirt.OnEnteredState += DecreaseWaterLevel;
        underwater.OnEnteredState += EnteredWater;
        underwater.OnExitState += ExitedWater;
        Rb.gravityScale = 4f;
        if (debugMode) GameManager.Instance.Inputs.Player.Enable();
    }

    #region WaterMechinic
    [Header("Water params")]
    [SerializeField] private float waterRegenAmount;
    [SerializeField] private float waterDegenAmount;
    [SerializeField] private float squirtWaterUseAmount;
    private float WaterLevel { get; set; }
    private bool _waterRegenActive;
    private bool _waterDegenActive;
    
    private void EnteredWater()
    {
        _waterRegenActive = true;
        _waterDegenActive = false;
    }

    private void ExitedWater()
    {
        _waterRegenActive = false;
        _waterDegenActive = true;
    }
    
    private void DecreaseWaterLevel()
    {
        if (_isUnderwater) return;
        WaterLevel -= squirtWaterUseAmount;
        GameManager.Instance.OnWaterLevelChanged?.Invoke(WaterLevel);
    }
    
    private void UpdateWaterLevel()
    {
        if (_waterRegenActive)
        {
            WaterLevel += waterRegenAmount * Time.deltaTime;
            if (WaterLevel > 1f) WaterLevel = 1f;            
        } else if (_waterDegenActive)
        {
            WaterLevel -= waterDegenAmount * Time.deltaTime;
            if (WaterLevel < 0f) WaterLevel = 0f;
        }
        GameManager.Instance.OnWaterLevelChanged?.Invoke(WaterLevel);
    }
    #endregion

    private bool CanWaterSquirt()
    {
        return GameManager.Instance.Inputs.Player.Jump.WasPressedThisFrame() && squirtWaterUseAmount <= WaterLevel;
    }

    private void Update()
    {
        CheckGround();
        CheckUnderWater();
        UpdateWaterLevel();
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
    
    [Serializable]
    public struct LeavingWaterParams
    {
        public float optimalAngle;
        public float force;
        public float maxVelocityForce;
        public float horizontalBoost;
        public float fullyVerticalBoost;
        public float minimumAngleFromTop;
        public float minimumBoost;
    }

    public void Squirt()
    {
        if (_isUnderwater)
        {
            Rb.velocity = transform.up * inWaterSquirtStrength;   
        }else
            Rb.velocity = transform.up * otherStatesWaterStrength;
    }
}