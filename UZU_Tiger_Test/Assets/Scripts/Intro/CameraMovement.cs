using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public Vector3 startPosition = new Vector3(1.54999995f, 11.1999998f, 17.6200008f); // 시작 위치
    public Vector3 targetPosition = new Vector3(-5.37f, 11.1999998f, 3.7f);  // 목표 위치
    public float duration = 15f; // 이동에 걸리는 시간
    private Coroutine moveCoroutine;

    void Start()
    {
        moveCoroutine = StartCoroutine(MoveCamera());
    }

    public void StopCameraMovement(float delay)
    {
        if (moveCoroutine != null)
        {
            StartCoroutine(DelayedStop(delay));
        }
    }

    private IEnumerator DelayedStop(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopCoroutine(moveCoroutine);
        moveCoroutine = null;
    }

    IEnumerator MoveCamera()
    {

        float elapsedTime = 0f; // 경과 시간

        while (elapsedTime < duration)
        {
            // 보간을 사용하여 시작 위치에서 목표 위치까지 이동
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null; // 한 프레임 기다림
        }

        // 이동이 끝나면 목표 위치로 고정
        transform.position = targetPosition;
    }
}