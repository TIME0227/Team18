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

    public GameObject dialoguePrefab; // Dialogue 프리팹
    public Transform dialogueParent;  // 대화 프리팹을 추가할 부모 오브젝트 (UI)

    // 각 상담사에 대한 NPC 초상화 이미지
    public Sprite[] npcPortraits; // Sprite 배열을 인스펙터에서 설정 가능

    public void Awake()
    {
        Dialogue = GetComponent<DialogueScript>();
        ds = new DataService("database.db"); // 데이터베이스 연결

        // DB에 있는 모든 현재 대화 요약본 데이터 가져오기
        for (int counselorId = 1; counselorId <= 4; counselorId++)
        {
            var sessionLogs = ds.GetSessionLog(counselorId); // 각 상담사에 대한 로그 가져오기
            Debug.Log($"SessionLogs for Counselor {counselorId}: Count = {sessionLogs.Count()}");

            // 모든 SessionLog에서 Summary를 사용해 각각의 대화 프리팹 생성
            foreach (var sessionLog in sessionLogs)
            {
                if (sessionLog != null)
                {
                    Debug.Log($"Checking SessionLog Id: {sessionLog.Id}, Summary: {sessionLog.Summary}");

                    if (!string.IsNullOrWhiteSpace(sessionLog.Summary))
                    {
                        createDialogueContent(sessionLog); // 각각의 대화 요약본을 대화 프리팹으로 출력
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
        // 아무 대화도 하지 않았다면
        if (string.IsNullOrWhiteSpace(sessionLog.Summary)) return;

        // Dialogue 프리팹에 text 내용 추가하여 Hierarchy에 clone 생성
        GameObject dialogueInstance = Instantiate(dialoguePrefab, dialogueParent);
        TMP_Text dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();

        if (dialogueText != null)
        {
            dialogueText.text = sessionLog.Summary;
        }
        else
        {
            Debug.Log("다이얼로그 프리팹 내 텍스트가 null입니다.");
        }

        // NPC 초상화 이미지 설정
        Image npcPortrait = dialogueInstance.GetComponentInChildren<DialogueScript>().NPC_portrait;

        if (npcPortrait != null && sessionLog.Counselor_id > 0 && sessionLog.Counselor_id <= npcPortraits.Length)
        {
            npcPortrait.sprite = npcPortraits[sessionLog.Counselor_id - 1]; // 배열 인덱스는 0부터 시작하므로 -1
        }
        else
        {
            Debug.Log("NPC 초상화 설정에 문제가 발생했습니다.");
        }

        // Debug.Log로 Id와 Counselor_id 출력
        Debug.Log($"SessionLog Id: {sessionLog.Id}, Counselor_id: {sessionLog.Counselor_id}, Summary: {sessionLog.Summary}");
    }

    public void OnClick_backtoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
