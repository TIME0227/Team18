using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class ReportManager : MonoBehaviour
{
    public GameObject Report;
    public Image NPC_portrait;
    public TMP_Text Report_Text;
    public TMP_Text Report_Date;
    public TMP_Text Report_NPCName;
    public Sprite[] npcPortraits;        // 각 상담사 초상화 이미지
    public Button showReportBtn;

    private int NPCflag = 0;
    // cog = 1 , kind = 3, cynical = 4

    DataService ds;
    DialogueManager dm;

    private void Awake()
    {
        ds = new DataService("database.db");
        dm = GetComponent<DialogueManager>();
        Report.SetActive(false);
    }

    public void Onclick_ShowCynicalReportBtn()
    {
        OnReportGenerated();
        showReportBtn.gameObject.SetActive(false);
    }

    #region CognitiveNPC
    public void OnClick_CreateReport_CognitiveNPC()
    {
        if (dm.canMakeReport_Cognitive)
        {
            NPCflag = 1;

            // NPC 이름 설정
            Report_NPCName.text = "인지치료 상담새";

            // NPC 초상화 이미지 설정
            NPC_portrait.sprite = npcPortraits[0];

            // 리포트 내용 화면에 출력
            ShowReport();

            // NPC 이름 설정
            PlayerPrefs.SetString("NPCName", "CognitiveNPC");

            // 리포트 생성 요청
            OpenAIController openAIController = FindObjectOfType<OpenAIController>();
            openAIController.SendReportRequestToAI();
        }
        else
        {
            Debug.Log("리포트 생성 불가");
        }

    }
    #endregion

    #region KindNPC
    public void OnClick_CreateReport_KindNPC()
    {
        if (dm.canMakeReport_Kind)
        {
            NPCflag = 3;

            // NPC 이름 설정
            Report_NPCName.text = "상냥한 상담새";

            // NPC 초상화 이미지 설정
            NPC_portrait.sprite = npcPortraits[2];

            // 리포트 내용 화면에 출력
            ShowReport();

            // NPC 이름 설정
            PlayerPrefs.SetString("NPCName", "KindNPC");

            // 리포트 생성 요청
            OpenAIController openAIController = FindObjectOfType<OpenAIController>();
            openAIController.SendReportRequestToAI();
        }
        else
        {
            Debug.Log("리포트 생성 불가");
        }
        
    }
    #endregion

    #region Cynical
    public void OnClick_CreateReport_CynicalNPC()
    {
        if (dm.canMakeReport_Cynical)
        {
            NPCflag = 4;

            // NPC 이름 설정
            Report_NPCName.text = "시니컬한 상담새";

            // NPC 초상화 이미지 설정
            NPC_portrait.sprite = npcPortraits[3];

            // 리포트 내용 화면에 출력
            ShowReport();

            // NPC 이름 설정
            PlayerPrefs.SetString("NPCName", "CynicalNPC");

            // 리포트 생성 요청
            OpenAIController openAIController = FindObjectOfType<OpenAIController>();
            openAIController.SendReportRequestToAI();
        }
        else
        {
            Debug.Log("리포트 생성 불가");
        }
    }

    private void OnReportGenerated()
    {
        var reportLogs = ds.GetReportLog(NPCflag);
        ReportLog reportLog = reportLogs.FirstOrDefault();

        if (reportLog != null)
        {
            // 리포트 텍스트 설정
            Report_Text.text = reportLog.Content;

            // 리포트 생성 날짜 설정
            Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

            // 리포트 내용 화면에 출력
            ShowReport();
        }

    }
    #endregion

    // 리포트 팝업 띄움
    public void ShowReport()
    {
        Report.SetActive(true);
    }

    // 닫기 버튼 눌러 리포트 팝업 내리기
    public void OnClickCloseReportBtn()
    {
        Report.SetActive(false);
        SceneManager.LoadScene("ReportStorage");
    }

}
