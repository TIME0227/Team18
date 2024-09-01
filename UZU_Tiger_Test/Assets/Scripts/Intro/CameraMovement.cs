using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public Vector3 startPosition = new Vector3(1.54999995f, 11.1999998f, 17.6200008f); // ���� ��ġ
    public Vector3 targetPosition = new Vector3(-5.37f, 11.1999998f, 3.7f);  // ��ǥ ��ġ
    public float duration = 15f; // �̵��� �ɸ��� �ð�
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

        float elapsedTime = 0f; // ��� �ð�

        while (elapsedTime < duration)
        {
            // ������ ����Ͽ� ���� ��ġ���� ��ǥ ��ġ���� �̵�
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null; // �� ������ ��ٸ�
        }

        // �̵��� ������ ��ǥ ��ġ�� ����
        transform.position = targetPosition;
    }
}