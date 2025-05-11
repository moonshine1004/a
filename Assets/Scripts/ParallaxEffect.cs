using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    //ī�޶�
    [SerializeField] private Camera camera;
    //�÷��̾� ��ġ
    [SerializeField] private Transform followTarget;
    //�̵� �ӵ��� ������ ��
    [SerializeField] float parallaxFactor=0.1f;

    //���� ��ġ ����
    Vector2 startingPosition;
    //������ ���۵ǰ� ���� ī�޶� �����̴� ����
    Vector2 camMoveSinceStart => (Vector2)camera.transform.position-startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + camMoveSinceStart*parallaxFactor;
        newPosition.y = 0;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
