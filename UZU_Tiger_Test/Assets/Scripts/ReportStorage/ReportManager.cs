using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using OpenCover.Framework.Model;
using System;
using System.Collections;

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


    #region CognitiveNPC
    public void OnClick_CreateReport_CognitiveNPC()
    {
        // NPC 이름 설정
        PlayerPrefs.SetString("NPCName", "CognitiveNPC");

        // 리포트 생성 요청
        OpenAIController openAIController = FindObjectOfType<OpenAIController>();
        openAIController.SendReportRequestToAI();

        // 리포트 생성 후 처리 대기
        StartCoroutine(WaitForReportResponse_Cognitive());

    }

    private IEnumerator WaitForReportResponse_Cognitive()
    {
        // 응답 대기 (필요한 시간만큼 대기)
        yield return new WaitForSeconds(1.0f); // 필요한 시간으로 설정

        // 리포트 생성 후 처리할 메서드 호출
        OnReportGenerated_Cognitive();
    }

    private void OnReportGenerated_Cognitive()
    {
        // 리포트 생성이 완료된 후 불러오기
        var reportLogs = ds.GetReportLog(1);
        ReportLog reportLog = reportLogs.FirstOrDefault();

        if (reportLog != null)
        {
            // 리포트 텍스트 설정
            Report_Text.text = reportLog.Content;

            // 리포트 생성 날짜 설정
            Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

            // NPC 이름 설정
            Report_NPCName.text = "인지치료 상담새";

            // NPC 초상화 이미지 설정
            NPC_portrait.sprite = npcPortraits[0];

            // 리포트 내용 화면에 출력
            ShowReport();
        }

        // 이벤트 구독 해제
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnReportGenerated_Cognitive;
    }
    #endregion

    #region KindNPC
    public void OnClick_CreateReport_KindNPC()
    {
        // NPC 이름 설정
        PlayerPrefs.SetString("NPCName", "KindNPC");

        // 리포트 생성 요청
        OpenAIController openAIController = FindObjectOfType<OpenAIController>();
        openAIController.SendReportRequestToAI();

        // 리포트 생성 후 처리 대기
        StartCoroutine(WaitForReportResponse());
    }

    private IEnumerator WaitForReportResponse()
    {
        // 응답 대기 (필요한 시간만큼 대기)
        yield return new WaitForSeconds(2.0f); // 필요한 시간으로 설정

        // 리포트 생성 후 처리할 메서드 호출
        OnReportGenerated_Kind();
    }

    private void OnReportGenerated_Kind()
    {
        // 리포트 생성이 완료된 후 불러오기
        var reportLogs = ds.GetReportLog(3);
        ReportLog reportLog = reportLogs.FirstOrDefault();

        if (reportLog != null)
        {
            // 리포트 텍스트 설정
            Report_Text.text = reportLog.Content;

            // 리포트 생성 날짜 설정
            Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

            // NPC 이름 설정
            Report_NPCName.text = "상냥한 상담새";

            // NPC 초상화 이미지 설정
            NPC_portrait.sprite = npcPortraits[2];

            // 리포트 내용 화면에 출력
            ShowReport();
        }

        // 이벤트 구독 해제
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnReportGenerated_Kind;
    }
    #endregion


    #region Cynical
    public void OnClick_CreateReport_CynicalNPC()
    {
        // NPC 이름 설정
        PlayerPrefs.SetString("NPCName", "CynicalNPC");

        // 리포트 생성 요청
        OpenAIController openAIController = FindObjectOfType<OpenAIController>();
        openAIController.SendReportRequestToAI();

        // 리포트 생성 후 처리 대기
        StartCoroutine(WaitForReportResponse_Cynical());
    }

    private IEnumerator WaitForReportResponse_Cynical()
    {
        // 응답 대기 (필요한 시간만큼 대기)
        yield return new WaitForSeconds(1.0f); // 필요한 시간으로 설정

        // 리포트 생성 후 처리할 메서드 호출
        OnReportGenerated_Cynical();
    }

    private void OnReportGenerated_Cynical()
    {
        // 리포트 생성이 완료된 후 불러오기
        var reportLogs = ds.GetReportLog(4);
        ReportLog reportLog = reportLogs.FirstOrDefault();

        if (reportLog != null)
        {
            // 리포트 텍스트 설정
            Report_Text.text = reportLog.Content;

            // 리포트 생성 날짜 설정
            Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

            // NPC 이름 설정
            Report_NPCName.text = "시니컬한 상담새";

            // NPC 초상화 이미지 설정
            NPC_portrait.sprite = npcPortraits[3];

            // 리포트 내용 화면에 출력
            ShowReport();
        }

        // 이벤트 구독 해제
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnReportGenerated_Cynical;
    }
    #endregion

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
