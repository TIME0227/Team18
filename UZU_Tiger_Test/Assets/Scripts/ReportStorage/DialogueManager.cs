using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.AssetImporters;

public class DialogueManager : MonoBehaviour
{
    DialogueScript Dialogue;
    DataService ds;

    public GameObject dialoguePrefab; // Dialogue 프리팹
    public Transform dialogueParent;  // 대화 프리팹을 추가할 부모 오브젝트 (UI)

    // 각 상담사에 대한 NPC 초상화 이미지
    public Sprite[] npcPortraits; // Sprite 배열을 인스펙터에서 설정 가능

    public bool canMakeReport_Kind = false;
    public bool canMakeReport_Cynical = false;
    public bool canMakeReport_Cognitive = false;

    string npcName;

    private GameObject dialogueInstance; // 추가: dialogueInstance 필드
    private SessionLog sessionLog; // 추가: sessionLog 필드

    public Button CRB_Kind;
    public Button CRB_Cynical;
    public Button CRB_Cognitive;

    public Sprite unActiveSprite;

    public void Awake()
    {
        Dialogue = GetComponent<DialogueScript>();
        ds = new DataService("database.db"); // 데이터베이스 연결

        Image CRB_KindBtnImg = CRB_Kind.GetComponent<Image>();
        Image CRB_CynicalBtnImg = CRB_Cynical.GetComponent<Image>();
        Image CRB_CognitiveBtnImg = CRB_Cognitive.GetComponent<Image>();

        PlayerPrefs.SetString("NPCName", "ReporterNPC");
        npcName = PlayerPrefs.GetString("NPCName");
        Debug.Log("DialogueManager : " + npcName);

        // DB에 있는 모든 현재 대화 요약본 데이터 가져오기
        for (int counselorId = 1; counselorId <= 4; counselorId++)
        {
            var sessionLogs = ds.GetSessionLog(counselorId); // 각 상담사에 대한 로그 가져오기
            Debug.Log($"SessionLogs for Counselor {counselorId}: Count = {sessionLogs.Count()}");

            // 모든 SessionLog에서 Summary를 사용해 각각의 대화 프리팹 생성
            foreach (var log in sessionLogs)
            {
                if (log != null)
                {
                    Debug.Log($"Checking SessionLog Id: {log.Id}, Summary: {log.Summary}");

                    if (!string.IsNullOrWhiteSpace(log.Summary))
                    {
                        createDialogueContent(log); // 각각의 대화 요약본을 대화 프리팹으로 출력
                    }
                    else
                    {
                        Debug.Log($"SessionLog Id {log.Id} has an empty Summary.");
                    }
                }
                else
                {
                    Debug.Log("SessionLog is null");
                }
            }
        }

        // 대화 내용 요약본이 5개 이상인지 확인
        canMakeReport_Kind = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("KindNPC"));
        canMakeReport_Cynical = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("CynicalNPC"));
        canMakeReport_Cognitive = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("CognitiveNPC"));

        Debug.Log("canMakeReport_Kind : " + canMakeReport_Kind);
        Debug.Log("canMakeReport_Cynical : " + canMakeReport_Cynical);
        Debug.Log("canMakeReport_Cognitive : " + canMakeReport_Cognitive);

        if (!canMakeReport_Kind)
            CRB_KindBtnImg.sprite = unActiveSprite;
        else
            CRB_KindBtnImg.sprite = Resources.Load<Sprite>("NPC_P/Circle Button - Pink Rose");

        if (!canMakeReport_Cynical)
            CRB_CynicalBtnImg.sprite = unActiveSprite;
        else
            CRB_CynicalBtnImg.sprite = Resources.Load<Sprite>("NPC_P/Circle Button - Green");

        if (!canMakeReport_Cognitive)
            CRB_CognitiveBtnImg.sprite = unActiveSprite;
        else
            CRB_CognitiveBtnImg.sprite = Resources.Load<Sprite>("NPC_P/Circle Button - Yellow");
    }

    private void Update()
    {
        if (dialogueInstance == null)
        {
            // dialogueInstance가 null일 경우 로그를 출력하고 return
            Debug.Log("dialogueInstance가 null입니다.");
            return;
        }
    }

    public void createDialogueContent(SessionLog log)
    {


        // 대화 내용이 없으면 리턴
        if (string.IsNullOrWhiteSpace(log.Summary)) return;

        // 대화 프리팹 인스턴스 생성
        dialogueInstance = Instantiate(dialoguePrefab, dialogueParent);
        TMP_Text dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();

        if (dialogueText != null)
        {
            dialogueText.text = log.Summary;
        }

        // NPC 초상화 이미지 설정
        Image npcPortrait = dialogueInstance.GetComponentInChildren<DialogueScript>().NPC_portrait;
        if (npcPortrait != null && log.Counselor_id > 0 && log.Counselor_id <= npcPortraits.Length)
        {
            npcPortrait.sprite = npcPortraits[log.Counselor_id - 1];
        }

        // 요약본 생성 날짜 설정
        dialogueInstance.GetComponentInChildren<DialogueScript>().Dialogue_Date.text = log.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

        // 현재 대화 로그 저장
        sessionLog = log;

        // 리포트 버튼 설정 (대화 내용 5개 이상일시 true, 아니면 false)
        Image CRB_BtnImg = CRB_Kind.GetComponent<Image>();
        CRB_BtnImg.sprite = unActiveSprite;
    }




    public void OnClick_backtoMain()
    {
        SceneManager.LoadScene("Main");
    }
}