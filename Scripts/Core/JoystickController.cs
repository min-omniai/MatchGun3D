using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum JoystickType
{
    NotUse,
    Fixed,
    Follow,
    RunningGame
}

public class JoystickController : MonoBehaviour
{
    [Header("조이스틱 타입")]
    [FormerlySerializedAs("_joystickType")]
    public JoystickType Mode = JoystickType.NotUse;
    [Header("조이스틱 반경")]
    [FormerlySerializedAs("_joystickBound")]
    public float JoystickBound = 50.0f;
    [Header("움직일 물체 - Rigidbody 필요")]
    public Rigidbody MoveObjectRigid = null;

    public float Threshold = 10.0f;

    [FormerlySerializedAs("_speed")]
    public float Speed = 5.0f;
    private float _currentSpeed = 0.0f;
    [FormerlySerializedAs("xBound")]
    public float XBound = 0.0f;
    public bool AutoRun = false;
    [FormerlySerializedAs("xSensitivity")]
    public float XSensitivity = 0.0f;
    [FormerlySerializedAs("xAcceletor")]
    public float XAccelerator = 0.0f;

    public bool UseAccelerate = false;
    public float Accelerate = 100.0f;

    private RectTransform _canvasRect;
    private RectTransform _joystick;
    private RectTransform _joystickHandle;
    [SerializeField]
    private Image _joystickImage;
    private Image _joystickHandleImage;

    private Vector3 _originPosition;
    private Vector3 _moveDirection = Vector3.zero;
    private float _originXPosition;
    private int _walkableMask;

    [HideInInspector]
    public bool CanMove = false;
    private bool _isButtonClicked = false;
    private bool _isMouseDown = false;

    public System.Action DownAction = null;
    public System.Action<Vector2> JoystickMoveAction = null;
    public System.Action UpAction = null;

    private void Awake()
    {
        _currentSpeed = 0.0f;
        _walkableMask = LayerMask.NameToLayer("Walkable");
        _canvasRect = GetComponent<RectTransform>();
        _joystick = _joystickImage.GetComponent<RectTransform>();
        _joystickHandle = _joystick.GetChild(0).GetComponent<RectTransform>();
        _joystickHandleImage = _joystickHandle.GetComponent<Image>();
        _joystickImage.enabled = false;
        _joystickHandleImage.enabled = false;

        AddDownEvent(() => _isMouseDown = true);
        AddUpEvent(() => _isMouseDown = false);

        switch (Mode)
        {
            case JoystickType.NotUse:
                _joystick.gameObject.SetActive(false);
                break;

            case JoystickType.Fixed:
                break;

            case JoystickType.Follow:
                break;
        }

        CanMove = true;
        _isButtonClicked = false;
    }

    private void Update()
    {
        if (!CanMove)
            return;

        if (UseAccelerate)
        {
            if (!_isMouseDown)
            {
                _currentSpeed = Mathf.Max(0, _currentSpeed - Accelerate * Time.deltaTime);

                CheckMoveDir();
            }

            MoveObjectRigid.position += _moveDirection * Time.deltaTime * _currentSpeed;
        }


        switch (Mode)
        {
            case JoystickType.NotUse:
                break;

            case JoystickType.Fixed:
                if (Input.GetMouseButtonDown(0))
                {
                    _joystickImage.enabled = true;
                    _joystickHandleImage.enabled = true;

                    _joystick.anchoredPosition = Input.mousePosition * 2688f / Screen.height;
                    _joystickHandle.anchoredPosition = Vector2.zero;
                    _originPosition = _joystick.anchoredPosition;

                    DownAction?.Invoke();
                }
                else if (Input.GetMouseButton(0) && !_isButtonClicked)
                {
                    _joystickHandle.anchoredPosition = Input.mousePosition * 2688f / Screen.height - _originPosition;
                    if (_joystickHandle.anchoredPosition.magnitude > JoystickBound)
                        _joystickHandle.anchoredPosition = _joystickHandle.anchoredPosition.normalized * JoystickBound;

                    if (_joystickHandle.anchoredPosition.magnitude < Threshold)
                        return;

                    JoystickMoveAction?.Invoke(_joystickHandle.anchoredPosition);

                    Vector3 dir = new Vector3(_joystickHandle.anchoredPosition.x, 0, _joystickHandle.anchoredPosition.y);
                    if (MoveObjectRigid != null)
                        Move(dir);
                }
                else if (Input.GetMouseButtonUp(0) && !_isButtonClicked)
                {
                    _joystickImage.enabled = false;
                    _joystickHandleImage.enabled = false;
                    UpAction?.Invoke();
                }
                break;

            case JoystickType.Follow:
                if (Input.GetMouseButtonDown(0))
                {
                    _joystickImage.enabled = true;
                    _joystickHandleImage.enabled = true;

                    _joystick.anchoredPosition = Input.mousePosition * 2688f / Screen.height;
                    _joystickHandle.anchoredPosition = Vector2.zero;
                    _originPosition = _joystick.anchoredPosition;

                    DownAction?.Invoke();
                }
                else if (Input.GetMouseButton(0) && !_isButtonClicked)
                {
                    _joystickHandle.anchoredPosition = Input.mousePosition * 2688f / Screen.height - _originPosition;
                    if (_joystickHandle.anchoredPosition.magnitude > JoystickBound)
                    {
                        _joystick.anchoredPosition = (Vector2)_originPosition + _joystickHandle.anchoredPosition - JoystickBound * _joystickHandle.anchoredPosition.normalized;
                        _joystickHandle.anchoredPosition = _joystickHandle.anchoredPosition.normalized * JoystickBound;
                        _originPosition = _joystick.anchoredPosition;
                    }

                    if (_joystickHandle.anchoredPosition.magnitude < Threshold)
                        return;

                    JoystickMoveAction?.Invoke(_joystickHandle.anchoredPosition);

                    Vector3 dir = new Vector3(_joystickHandle.anchoredPosition.x, 0, _joystickHandle.anchoredPosition.y);
                    if (MoveObjectRigid != null)
                        Move(dir);
                }
                else if (Input.GetMouseButtonUp(0) && !_isButtonClicked)
                {
                    _joystickImage.enabled = false;
                    _joystickHandleImage.enabled = false;
                    UpAction?.Invoke();
                }
                break;
        }
    }

    public void AddDownEvent(System.Action action)
    {
        DownAction -= action;
        DownAction += action;
    }

    public void AddMoveEvent(System.Action<Vector2> action)
    {
        JoystickMoveAction -= action;
        JoystickMoveAction += action;
    }

    public void AddUpEvent(System.Action action)
    {
        UpAction -= action;
        UpAction += action;
    }

    private void Move(Vector3 dir)
    {
        _moveDirection = dir.normalized;

        MoveObjectRigid.rotation = Quaternion.LookRotation(_moveDirection);

        CheckMoveDir();

        if (UseAccelerate)
            _currentSpeed = Mathf.Min(Speed, _currentSpeed + Time.deltaTime * Accelerate);
        else
            MoveObjectRigid.position += _moveDirection * Time.deltaTime * Speed;
    }

    private void CheckMoveDir()
    {
        bool moveForward = Physics.Raycast(MoveObjectRigid.position + Vector3.forward * Define.Bound + Vector3.up * 50, Vector3.down, 100, 1 << _walkableMask);
        bool moveBack = Physics.Raycast(MoveObjectRigid.position + Vector3.back * Define.Bound + Vector3.up * 50, Vector3.down, 100, 1 << _walkableMask);
        bool moveRight = Physics.Raycast(MoveObjectRigid.position + Vector3.right * Define.Bound + Vector3.up * 50, Vector3.down, 100, 1 << _walkableMask);
        bool moveLeft = Physics.Raycast(MoveObjectRigid.position + Vector3.left * Define.Bound + Vector3.up * 50, Vector3.down, 100, 1 << _walkableMask);

        if (!moveForward && _moveDirection.z >= 0) _moveDirection.z = 0;
        if (!moveBack && _moveDirection.z < 0) _moveDirection.z = 0;
        if (!moveRight && _moveDirection.x >= 0) _moveDirection.x = 0;
        if (!moveLeft && _moveDirection.x < 0) _moveDirection.x = 0;
    }
}
