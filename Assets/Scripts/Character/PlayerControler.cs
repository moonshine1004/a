using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    //프로퍼티(Property): C#에서 클래스의 멤버 변수(필드)에 대한 접근을 제어하는 방법을 제공하는 기능
    //프로퍼티를 사용하면 필드에 대한 읽기와 쓰기를 캡슐화-->필드 값에 대한 유효성 검사 혹은 다른 로직을 실행할 수 있게함
    //프로퍼티에는 get과 set이라는 키워드가 있음
    //get: 멤버 변수(필드)의 값을 가져오는 키워드-->무조건 리턴 키워드가 있어야 함
    //set: 멤버 변수(필드)의 값을 수정하는 키워드-->필드=value키워드를 통해서 바꾸려는 키워드를 갱신해야함

    //이동, 점프, 스프린트에 대한 변수
    //[SerializeField]: 인스펙터창에서 private 변수를 수정할 수 있게 함
    [SerializeField] private float walkspeed = 8;
    [SerializeField] private float sprintspeed = 30;
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private int MaxJumpCount = 1;

    private Vector3 inputDirection = Vector3.zero;
    //점프한 횟수를 저장하는 변수
    private int _currentJumpCount;
    //움직임 여부 상태 선언
    private bool isMoving=false;
    //isMoving에 대한 프로퍼티
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
            _animator.SetBool(AnimationStrings.IsMoving, isMoving);
        }
    }
    //스프린트 상태의 기본 상태를 false로 초기화
    private bool isSprint=false;
    //isSprint에 대한 프로퍼티
    public bool IsSprint
    {
        get
        {
            return isSprint;
        }
        set
        {
            isSprint = value;
            _animator.SetBool(AnimationStrings.IsSpringt, isSprint);
        }
    }

    //리지드 바디 구조체 선언
    Rigidbody2D _rb;
    //애니메이터
    Animator _animator;
    //TouchingDirection 클래스를 선언
    TouchingDirections _touchingDirection;

    void Start()
    {
        _rb= GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirection = GetComponent<TouchingDirections>();
    }
    void Update()
    {
        //객체의 y속도가 0보다 작고(추락 중) 객체가 땅에 닿았을 때 점프 횟수를 0으로 초기화
        if (_rb.linearVelocityY<0 &&_touchingDirection.IsGrounded)
        {
            _currentJumpCount = 0;
        }
        float currentSpeed=isSprint?sprintspeed:walkspeed;
        currentSpeed = _touchingDirection.IsWall ? 0 : currentSpeed;
        _rb.linearVelocity = new Vector2(inputDirection.x * currentSpeed, _rb.linearVelocityY);
        //애니메이션 state를 Jump로 바꿈
        _animator.SetFloat(AnimationStrings.yVelocity, _rb.linearVelocityY);
    }
    public void OnMoveInput(InputAction.CallbackContext callback)
    {
        inputDirection = callback.ReadValue<Vector2>();
        IsMoving = inputDirection != Vector3.zero;
        OnSetDirection();
    }

    private void OnSetDirection()
    {
        if (transform.localScale.x > 0 && inputDirection.x < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else if(transform.localScale.x < 0 && inputDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    //스프린트에 대한 콜백을 받아 isSprint 초기화
    public void OnSprintInput(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            IsSprint = true;
        }
        else if(callback.canceled)
        {
            IsSprint = false;
        }
    }
    //점프에 대한 콜백을 받아 
    public void OnJumpInput(InputAction.CallbackContext callback)
    {
        if (callback.started&&_currentJumpCount<MaxJumpCount)
        {
            //애니메이션 state를 Jump로 바꿈
            _animator.SetTrigger(AnimationStrings.Jump);
            _currentJumpCount++;
            _rb.linearVelocity = new Vector2(inputDirection.x, jumpPower);
        }
    }
}
