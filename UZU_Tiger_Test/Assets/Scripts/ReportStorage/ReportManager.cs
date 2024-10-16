using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using OpenCover.Framework.Model;
using System;

public class ReportManager : MonoBehaviour
{
    public GameObject Report;
    public Image NPC_portrait;
    public TMP_Text Report_Text;
    public TMP_Text Report_Date;
    public TMP_Text Report_NPCName;
    public Sprite[] npcPortraits;        // 각 상담사 초상화 이미지

    DataService ds;

    private void Awake()
    {
        ds = new DataService("database.db");
        Report.SetActive(false);
    }

    // 리포트 생성 & 저장 요청 메서드

    public void OnClick_CreateReport_CognitiveNPC()
    {
        // 각 NPCname별 혹은 counsler_id에 따라 세팅달리하기
        // 지금은 테스트용이라 CognitiveNPC로만
        PlayerPrefs.SetString("NPCName", "CognitiveNPC");

        // 리포트 생성 및 저장 요청
        FindObjectOfType<OpenAIController>().SendReportRequestToAI();

        // 생성된 리포트 불러오기
        var reportLogs = ds.GetReportLog(1);


        // 맨 첫번째 리포트인듯?
        ReportLog reportLog = reportLogs.FirstOrDefault();


        // 리포트 텍스트 설정
        Report_Text.text = reportLog.Content;

        // NPC 이름 설정
        Report_NPCName.text = "인지치료 상담새";

        // NPC 초상화 이미지 설정
        Image npcPortrait = NPC_portrait;
        npcPortrait.sprite = npcPortraits[0];


        // 리포트 내용 화면에 출력
        ShowReport();

    }

    public void OnClick_CreateReport_KindNPC()
    {
        // 각 NPCname 따라 세팅 달리하기
        PlayerPrefs.SetString("NPCName", "KindNPC");

        // 리포트 생성 및 저장 요청
        FindObjectOfType<OpenAIController>().SendReportRequestToAI();

        // 생성된 리포트 불러오기
        var reportLogs = ds.GetReportLog(3);

        // 맨 첫번째 리포트인듯?
        ReportLog reportLog = reportLogs.FirstOrDefault();

        foreach (var log in ds.GetReportLog(3))
        {
            Debug.Log(log.ToString());
            int id = log.Id;
            string report_content = log.Content;
            string summary = log.Summary;
            DateTime created_at = log.Created_at;
        }

        // 리포트 텍스트 설정
        Report_Text.text = reportLog.Content;

        // 리포트 생성 날짜 설정
        Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

        // NPC 이름 설정
        Report_NPCName.text = "상냥한 상담새";


        // NPC 초상화 이미지 설정
        Image npcPortrait = NPC_portrait;
        npcPortrait.sprite = npcPortraits[2];


        // 리포트 내용 화면에 출력
        ShowReport();

    }

    public void OnClick_CreateReport_CynicalNPC()
    {
        // 각 NPCname 따라 세팅 달리하기
        PlayerPrefs.SetString("NPCName", "CynicalNPC");

        // 리포트 생성 및 저장 요청
        FindObjectOfType<OpenAIController>().SendReportRequestToAI();

        // 생성된 리포트 불러오기
        var reportLogs = ds.GetReportLog(4);

/*        // 리포트가 없을 경우에 대한 예외 처리
        if (reportLogs == null || !reportLogs.Any())
        {
            Debug.LogError("Report logs are empty or null.");
            return;  // 리포트가 없으면 실행 중단
        }*/

        // 맨 첫번째 리포트인듯?
        ReportLog reportLog = reportLogs.FirstOrDefault();

        /*        // 리포트가 null일 경우 처리
                if (reportLog == null)
                {
                    Debug.LogError("No report log found.");
                    return;  // 리포트가 없으면 실행 중단
                }*/

        // 리포트 텍스트 설정
        Report_Text.text = reportLog.Content;

        // NPC 이름 설정
        Report_NPCName.text = "시니컬한 상담새";

        // NPC 초상화 이미지 설정
        Image npcPortrait = NPC_portrait;
        npcPortrait.sprite = npcPortraits[3];


        // 리포트 내용 화면에 출력
        ShowReport();

    }

    public void OnClick_TestReport()
    {
        ShowReport();
    }

    // 리포트 팝업 띄움
    public void ShowReport()
    {
        Report.SetActive(true);
    }

    // 닫기 버튼 눌러 리포트 팝업 내리기
    public void OnClickCloseReportBtn()
    {
        Report.SetActive(false);
    }

}
