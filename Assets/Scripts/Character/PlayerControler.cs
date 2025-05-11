using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    //������Ƽ(Property): C#���� Ŭ������ ��� ����(�ʵ�)�� ���� ������ �����ϴ� ����� �����ϴ� ���
    //������Ƽ�� ����ϸ� �ʵ忡 ���� �б�� ���⸦ ĸ��ȭ-->�ʵ� ���� ���� ��ȿ�� �˻� Ȥ�� �ٸ� ������ ������ �� �ְ���
    //������Ƽ���� get�� set�̶�� Ű���尡 ����
    //get: ��� ����(�ʵ�)�� ���� �������� Ű����-->������ ���� Ű���尡 �־�� ��
    //set: ��� ����(�ʵ�)�� ���� �����ϴ� Ű����-->�ʵ�=valueŰ���带 ���ؼ� �ٲٷ��� Ű���带 �����ؾ���

    //�̵�, ����, ������Ʈ�� ���� ����
    //[SerializeField]: �ν�����â���� private ������ ������ �� �ְ� ��
    [SerializeField] private float walkspeed = 8;
    [SerializeField] private float sprintspeed = 30;
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private int MaxJumpCount = 1;

    private Vector3 inputDirection = Vector3.zero;
    //������ Ƚ���� �����ϴ� ����
    private int _currentJumpCount;
    //������ ���� ���� ����
    private bool isMoving=false;
    //isMoving�� ���� ������Ƽ
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
    //������Ʈ ������ �⺻ ���¸� false�� �ʱ�ȭ
    private bool isSprint=false;
    //isSprint�� ���� ������Ƽ
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

    //������ �ٵ� ����ü ����
    Rigidbody2D _rb;
    //�ִϸ�����
    Animator _animator;
    //TouchingDirection Ŭ������ ����
    TouchingDirections _touchingDirection;

    void Start()
    {
        _rb= GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirection = GetComponent<TouchingDirections>();
    }
    void Update()
    {
        //��ü�� y�ӵ��� 0���� �۰�(�߶� ��) ��ü�� ���� ����� �� ���� Ƚ���� 0���� �ʱ�ȭ
        if (_rb.linearVelocityY<0 &&_touchingDirection.IsGrounded)
        {
            _currentJumpCount = 0;
        }
        float currentSpeed=isSprint?sprintspeed:walkspeed;
        currentSpeed = _touchingDirection.IsWall ? 0 : currentSpeed;
        _rb.linearVelocity = new Vector2(inputDirection.x * currentSpeed, _rb.linearVelocityY);
        //�ִϸ��̼� state�� Jump�� �ٲ�
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
    //������Ʈ�� ���� �ݹ��� �޾� isSprint �ʱ�ȭ
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
    //������ ���� �ݹ��� �޾� 
    public void OnJumpInput(InputAction.CallbackContext callback)
    {
        if (callback.started&&_currentJumpCount<MaxJumpCount)
        {
            //�ִϸ��̼� state�� Jump�� �ٲ�
            _animator.SetTrigger(AnimationStrings.Jump);
            _currentJumpCount++;
            _rb.linearVelocity = new Vector2(inputDirection.x, jumpPower);
        }
    }
}
