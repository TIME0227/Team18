using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    // �̵� �ӵ��� �����ϴ� ����
    public float moveSpeed = 2f;

    // Ÿ�ϵ��� ��ġ�� ������ �迭
    public Transform[] tiles;

    // ���� ��ǥ Ÿ�� �ε���
    private int currentTargetIndex = 0;

    // ���� ��ǥ Ÿ�� ��ġ
    private Vector3 currentTargetPosition;

    // ���� ����
    private int randomNum;

    void Start()
    {
        if (tiles.Length > 0)
        {
            // ù ��° Ÿ���� ��ǥ ��ġ�� ����
            currentTargetPosition = tiles[currentTargetIndex].position;
            currentTargetPosition.y = -6;
        }
    }

    void Update()
    {
        if (tiles.Length == 0)
            return;

        // ���� ������Ʈ�� ���� ��ǥ ��ġ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);

        // ��ǥ ��ġ�� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, currentTargetPosition) < 0.1f)
        {
            // ���� Ÿ�Ϸ� ��ǥ ��ġ�� ����
            currentTargetIndex++;

            // Ÿ�� �迭�� ���� �����ϸ� ó������ ���ư���
            if (currentTargetIndex >= tiles.Length)
            {
                randomNum = Random.Range(0, tiles.Length);
                currentTargetIndex = randomNum;
            }

            currentTargetPosition = tiles[currentTargetIndex].position;
            currentTargetPosition.y = -6;
            Debug.Log($"���� Ÿ�� �ѹ� : {tiles[currentTargetIndex]}");
        }
    }
}
