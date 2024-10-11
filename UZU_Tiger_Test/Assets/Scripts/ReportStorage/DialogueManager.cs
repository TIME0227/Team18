using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    DialogueScript Dialogue;
    DataService ds;

    public GameObject dialoguePrefab; // Dialogue ������
    public Transform dialogueParent;  // ��ȭ �������� �߰��� �θ� ������Ʈ (UI)

    // �� ���翡 ���� NPC �ʻ�ȭ �̹���
    public Sprite[] npcPortraits; // Sprite �迭�� �ν����Ϳ��� ���� ����

    public void Awake()
    {
        Dialogue = GetComponent<DialogueScript>();
        ds = new DataService("database.db"); // �����ͺ��̽� ����

        // DB�� �ִ� ��� ���� ��ȭ ��ົ ������ ��������
        for (int counselorId = 1; counselorId <= 4; counselorId++)
        {
            var sessionLogs = ds.GetSessionLog(counselorId); // �� ���翡 ���� �α� ��������
            Debug.Log($"SessionLogs for Counselor {counselorId}: Count = {sessionLogs.Count()}");

            // ��� SessionLog���� Summary�� ����� ������ ��ȭ ������ ����
            foreach (var sessionLog in sessionLogs)
            {
                if (sessionLog != null)
                {
                    Debug.Log($"Checking SessionLog Id: {sessionLog.Id}, Summary: {sessionLog.Summary}");

                    if (!string.IsNullOrWhiteSpace(sessionLog.Summary))
                    {
                        createDialogueContent(sessionLog); // ������ ��ȭ ��ົ�� ��ȭ ���������� ���
                    }
                    else
                    {
                        Debug.Log($"SessionLog Id {sessionLog.Id} has an empty Summary.");
                    }
                }
                else
                {
                    Debug.Log("SessionLog is null");
                }
            }
        }
    }

    public void createDialogueContent(SessionLog sessionLog)
    {
        // �ƹ� ��ȭ�� ���� �ʾҴٸ�
        if (string.IsNullOrWhiteSpace(sessionLog.Summary)) return;

        // Dialogue �����տ� text ���� �߰��Ͽ� Hierarchy�� clone ����
        GameObject dialogueInstance = Instantiate(dialoguePrefab, dialogueParent);
        TMP_Text dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();

        if (dialogueText != null)
        {
            dialogueText.text = sessionLog.Summary;
        }
        else
        {
            Debug.Log("���̾�α� ������ �� �ؽ�Ʈ�� null�Դϴ�.");
        }

        // NPC �ʻ�ȭ �̹��� ����
        Image npcPortrait = dialogueInstance.GetComponentInChildren<DialogueScript>().NPC_portrait;

        if (npcPortrait != null && sessionLog.Counselor_id > 0 && sessionLog.Counselor_id <= npcPortraits.Length)
        {
            npcPortrait.sprite = npcPortraits[sessionLog.Counselor_id - 1]; // �迭 �ε����� 0���� �����ϹǷ� -1
        }
        else
        {
            Debug.Log("NPC �ʻ�ȭ ������ ������ �߻��߽��ϴ�.");
        }

        // Debug.Log�� Id�� Counselor_id ���
        Debug.Log($"SessionLog Id: {sessionLog.Id}, Counselor_id: {sessionLog.Counselor_id}, Summary: {sessionLog.Summary}");
    }

    public void OnClick_backtoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
