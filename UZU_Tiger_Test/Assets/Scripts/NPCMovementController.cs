using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    // 이동 속도를 조절하는 변수
    public float moveSpeed = 2f;

    // 타일들의 위치를 저장할 배열
    public Transform[] tiles;

    // 현재 목표 타일 인덱스
    private int currentTargetIndex = 0;

    // 현재 목표 타일 위치
    private Vector3 currentTargetPosition;

    // 랜덤 정수
    private int randomNum;

    void Start()
    {
        if (tiles.Length > 0)
        {
            // 첫 번째 타일을 목표 위치로 설정
            currentTargetPosition = tiles[currentTargetIndex].position;
            currentTargetPosition.y = -6;
        }
    }

    void Update()
    {
        if (tiles.Length == 0)
            return;

        // 게임 오브젝트를 현재 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);

        // 목표 위치에 도달했는지 확인
        if (Vector3.Distance(transform.position, currentTargetPosition) < 0.1f)
        {
            // 다음 타일로 목표 위치를 갱신
            currentTargetIndex++;

            // 타일 배열의 끝에 도달하면 처음으로 돌아가기
            if (currentTargetIndex >= tiles.Length)
            {
                randomNum = Random.Range(0, tiles.Length);
                currentTargetIndex = randomNum;
            }

            currentTargetPosition = tiles[currentTargetIndex].position;
            currentTargetPosition.y = -6;
            Debug.Log($"현재 타일 넘버 : {tiles[currentTargetIndex]}");
        }
    }
}
