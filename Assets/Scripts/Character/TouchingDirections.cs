using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D _touchingCollider;
    //�ٴڰ� �ε��� ������� ���� �迭 ����
    //��ü �ݶ��̴��� �����Ͽ� �����Ÿ� ��ŭ ������ �浹�� �̸� ����
    RaycastHit2D[] _groundHits=new RaycastHit2D[5];
    //�ٴڰ� Cast�� �Ÿ� ��
    [SerializeField] private float groundHitDistance = 0.05f;
    //�ٴڰ��� �浹 ���� ���� boolean
    private bool _isGround = false;
    private Animator _animator;
    //���� ������ ���� ������Ƽ: �ִϸ��̼��� IsGrounded �Ķ���͸� _isGrounded�� ���� �޾ƿ� �ʱ�ȭ
    public bool IsGrounded
    {
        get { return _isGround;}
        set { 
                _animator.SetBool(AnimationStrings.IsGrounded, IsGrounded);
                _isGround = value; 
            }
    }
    //���̶� �浹�� ����� ���� �迭 ����
    RaycastHit2D[] _wallHits = new RaycastHit2D[5];
    //���� Cast�� �Ÿ� ��
    [SerializeField] private float wallHitDistance = 0.2f;
    //������ �浹 ����
    private bool _isWall = false;
    //������ �浹 ���θ� �Ǵ��ϴ� ������Ƽ
    public bool IsWall
    {
        get { return _isWall; }
        set { _isWall = value; }
    }
    //���� �浹�� ����
    //�б� ���� ������Ƽ(read only property)
    private Vector2 WallCheckDirection => gameObject.transform.localPosition.x > 0 ? Vector2.right : Vector2.left;

    void Start()
    {
        _touchingCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        //�ٴڰ��� �浹 ����(�ο￬��)
        IsGrounded=_touchingCollider.Cast(Vector2.down, _groundHits, groundHitDistance)>0;
        IsWall = _touchingCollider.Cast(WallCheckDirection, _wallHits, wallHitDistance)>0;


    }
}
