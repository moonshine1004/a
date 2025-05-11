using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    //카메라
    [SerializeField] private Camera camera;
    //플레이어 위치
    [SerializeField] private Transform followTarget;
    //이동 속도를 나누는 값
    [SerializeField] float parallaxFactor=0.1f;

    //시작 위치 저장
    Vector2 startingPosition;
    //게임이 시작되고 나서 카메라가 움직이는 방향
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
