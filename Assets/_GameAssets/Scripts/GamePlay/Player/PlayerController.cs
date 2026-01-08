using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJumped;
    public event Action<PlayerState> OnPlayerStateChanged;

    [Header("References")]
    [SerializeField] private Transform _orientationTransform;

    [Header("Movement Settings")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _maxSpeed;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    [SerializeField] private float _airDrag;
    [SerializeField] private bool _canJump = true;

    [Header("Slide Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private float _slideDrag;

    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight = 2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;

    private float _startYScale;
    private StateController _stateController;
    private Rigidbody _playerRigidbody;
    
    // HATALI TANIMLAMALAR DÜZELTİLDİ
    private float _startingMovementSpeed, _startingJumpForce; // float tipi eklendi
    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    private bool _isSliding = false; // Mükerrer (duplicate) tanım silindi

    private void Awake()
    {
        _stateController = GetComponent<StateController>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
        _startYScale = transform.localScale.y;
        
        // Başlangıç değerleri kaydedildi
        _startingMovementSpeed = _movementSpeed;
        _startingJumpForce = _jumpForce;
    }

    private void Start()
    {
        _canJump = true;
    }

    private void Update()
    {
        if(GameManager.Instance.GetCurrentGameState() != GameState.Play &&
           GameManager.Instance.GetCurrentGameState() != GameState.Resume)
           {
            return;
           }

        SetInputs();
        SetState();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.GetCurrentGameState() != GameState.Play &&
           GameManager.Instance.GetCurrentGameState() != GameState.Resume)
           {
            return;
           }
           
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSliding = false;
        }

        if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown);
        }
    }

    private void SetState()
    {
        var movementDir = GetMovementDirection();
        var isGrounded = IsGrounded();
        var currentState = _stateController.GetCurrentState();

        var newState = movementDir switch
        {
            _ when movementDir == Vector3.zero && isGrounded && !_isSliding => PlayerState.Idle,
            _ when movementDir != Vector3.zero && isGrounded && !_isSliding => PlayerState.Move,
            _ when movementDir != Vector3.zero && isGrounded && _isSliding => PlayerState.Slide,
            _ when movementDir == Vector3.zero && isGrounded && _isSliding => PlayerState.SlideIdle,
            _ when !_canJump && !isGrounded => PlayerState.Jump,
            _ => currentState,
        };

        if (newState != currentState)
        {
            _stateController.ChangeState(newState);
            OnPlayerStateChanged?.Invoke(newState);
        }
    }

    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput +
                             _orientationTransform.right * _horizontalInput;

        float forceMultiplier = _stateController.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => _slideMultiplier,
            PlayerState.Jump => _airMultiplier,
            _ => 1f
        };

        _playerRigidbody.AddForce(
            _movementDirection.normalized * _movementSpeed * forceMultiplier,
            ForceMode.Force
        );
    }

    private void SetPlayerDrag()
    {
        // Unity 6 linearDamping kullanır
        _playerRigidbody.linearDamping = _stateController.GetCurrentState() switch
        {
            PlayerState.Move => _groundDrag,
            PlayerState.Slide => _slideDrag,
            PlayerState.Jump => _airDrag,
            _ => 0f
        };
    }

    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);

        if (flatVelocity.magnitude > _maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _maxSpeed;
            _playerRigidbody.linearVelocity = new Vector3(limitedVelocity.x, _playerRigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void SetPlayerJumping()
    {
        OnPlayerJumped?.Invoke(); 
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        _canJump = true;
    }

    #region Helper Functions

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
    }
    
    private Vector3 GetMovementDirection()
    {
        Vector3 dir = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        return dir.normalized;
    }

    // Buğdaylar için hız değiştirme
    public void SetMovementSpeed(float speed, float duration)
    {
        _movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed), duration);
    }

    private void ResetMovementSpeed()
    {
        _movementSpeed = _startingMovementSpeed; 
    }

    // Buğdaylar için zıplama kuvveti değiştirme
    public void SetJumpForce(float force, float duration)
    {
        _jumpForce += force;
        Invoke(nameof(ResetJumpForce), duration);
    }

    private void ResetJumpForce()
    {
        _jumpForce = _startingJumpForce; 
    }

    public Rigidbody GetPlayerRigidbody()
    {
        return _playerRigidbody;
    }

    #endregion
}