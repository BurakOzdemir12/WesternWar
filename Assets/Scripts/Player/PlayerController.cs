using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


[RequireComponent(typeof(CrowdSystem))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private LevelDataSo currentLevelSo;
    [SerializeField] private CrowdSystem crowdSystem;
    [SerializeField] private PlayerAnimator playerAnimator;

    public enum MovementMode
    {
        ForwardOnly,
        StrafeOnly
    }

    [Header("Player Movement Settings")] [SerializeField]
    private float speed = 5f;

    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private float xClampRange = 5f;

    private Vector3 _clickedScreenPos;
    private Vector3 _clickedPlayerPos;

    private bool _isTouching;
    private bool _isInGame = false;
    private bool _canMoveForward = false;

    private float _lastStrafeSent = float.NaN;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        crowdSystem = GetComponent<CrowdSystem>();
        playerAnimator = GetComponent<PlayerAnimator>();

        _canMoveForward = currentLevelSo != null && currentLevelSo.canPlayerMoveForward;
    }

    private void OnEnable()
    {
        TouchHandler.OnTouchingChanged += HandleTouchState;
        TouchHandler.OnPointerPressed += HandlePointerPressed;
        GameManager.OnGameStateChanged += HandleGameStateChange;
    }


    private void Start()
    {
        playerAnimator.SetRunState(_canMoveForward);
        if (!_canMoveForward) SetStrafe(0f);
    }

    private void Update()
    {
        if (!_isInGame) return;

        if (_canMoveForward) MoveForward();

        if (!_isTouching) return;

        HandleMovement();
    }

    private void HandleGameStateChange(GameManager.GameState gameState)
    {
        _isInGame = gameState == GameManager.GameState.Game;
        if (!_isInGame) SetStrafe(0f);
    }


    private void StopMoving()
    {
        _isInGame = false;
        playerAnimator.SetIdleState(!_isInGame);
    }

    private void HandleTouchState(bool isPressed)
    {
        _isTouching = isPressed;
        if (!isPressed) SetStrafe(0f);
    }

    private void HandlePointerPressed(Vector2 screenPos)
    {
        _clickedScreenPos = screenPos;
        _clickedPlayerPos = transform.position;
    }

    private void HandleMovement()
    {
        if (!_isTouching) return;
        // Kaydırma hesabı
        Vector2 pointerPosition = Mouse.current != null && Mouse.current.leftButton.isPressed
            ? Mouse.current.position.ReadValue()
            : Touchscreen.current?.primaryTouch.position.ReadValue() ?? Vector2.zero;

        float xScreenDifference = pointerPosition.x - _clickedScreenPos.x;
        xScreenDifference /= Screen.width; // normalize
        xScreenDifference *= slideSpeed;

        float strafeX = Mathf.Clamp(xScreenDifference, -1f, 1f);
        SetStrafe(_canMoveForward ? 0f : strafeX);


        Vector3 newPosition = transform.position;
        // newPosition.x = _clickedPlayerPos.x + xScreenDifference;

        newPosition.x += xScreenDifference;
        newPosition.x = Mathf.Clamp(newPosition.x, -xClampRange + crowdSystem.GetCrowdRadius(),
            xClampRange - crowdSystem.GetCrowdRadius());

        transform.position = newPosition;
    }

    private void MoveForward()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
  private void SetStrafe(float value)
  {
      // Eğer değer değişmemişse veya çok küçük bir fark varsa çık
      if (!float.IsNaN(_lastStrafeSent) && Mathf.Abs(_lastStrafeSent - value) < 0.01f)
      {
          return;
      }
  
      // Animasyon parametresini güncelle
      playerAnimator.SetStrafeState(value);
      _lastStrafeSent = value;
  }
    private void OnDisable()
    {
        TouchHandler.OnTouchingChanged -= HandleTouchState;
        TouchHandler.OnPointerPressed -= HandlePointerPressed;
        GameManager.OnGameStateChanged -= HandleGameStateChange;
    }
}