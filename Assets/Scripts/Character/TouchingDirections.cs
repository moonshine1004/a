using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D _touchingCollider;
    //바닥과 부딪힌 결과물에 대한 배열 선언
    //객체 콜라이더를 복제하여 일정거리 만큼 움직여 충돌을 미리 감지
    RaycastHit2D[] _groundHits=new RaycastHit2D[5];
    //바닥과 Cast의 거리 값
    [SerializeField] private float groundHitDistance = 0.05f;
    //바닥과의 충돌 여부 저장 boolean
    private bool _isGround = false;
    private Animator _animator;
    //지면 착지에 대한 프로퍼티: 애니메이션의 IsGrounded 파라미터를 _isGrounded로 부터 받아와 초기화
    public bool IsGrounded
    {
        get { return _isGround;}
        set { 
                _animator.SetBool(AnimationStrings.IsGrounded, IsGrounded);
                _isGround = value; 
            }
    }
    //벽이랑 충돌한 결과에 대한 배열 선언
    RaycastHit2D[] _wallHits = new RaycastHit2D[5];
    //벽과 Cast의 거리 값
    [SerializeField] private float wallHitDistance = 0.2f;
    //벽과의 충돌 여부
    private bool _isWall = false;
    //벽과의 충돌 여부를 판단하는 프로퍼티
    public bool IsWall
    {
        get { return _isWall; }
        set { _isWall = value; }
    }
    //벽과 충돌한 방향
    //읽기 전용 프로퍼티(read only property)
    private Vector2 WallCheckDirection => gameObject.transform.localPosition.x > 0 ? Vector2.right : Vector2.left;

    void Start()
    {
        _touchingCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        //바닥과의 충돌 여부(부울연산)
        IsGrounded=_touchingCollider.Cast(Vector2.down, _groundHits, groundHitDistance)>0;
        IsWall = _touchingCollider.Cast(WallCheckDirection, _wallHits, wallHitDistance)>0;


    }
}
